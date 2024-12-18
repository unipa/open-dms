﻿/*
 *  Copyright 2012-2021 The Pkcs11Interop Project
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

/*
 *  Written for the Pkcs11Interop project by:
 *  Jaroslav IMRICH <jimrich@jimrich.sk>
 */

using System;
using System.Collections.Generic;
using global::Net.Pkcs11Interop.Common;
using global::Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;


namespace Core.DigitalSignature.Pkcs11
{
        /// <summary>
        /// Helper methods for HighLevelAPI tests.
        /// </summary>
        public static class Helpers
        {
            /// <summary>
            /// Finds slot containing the token that matches criteria specified in Settings class
            /// </summary>
            /// <param name='pkcs11Library'>Initialized PKCS11 wrapper</param>
            /// <returns>Slot containing the token that matches criteria</returns>
            public static List<ISlot> GetUsableSlot(IPkcs11Library pkcs11Library)
            {
                // Get list of available slots with token present
                List<ISlot> slots = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent);

            //                Assert.IsNotNull(slots);
            //                Assert.IsTrue(slots.Count > 0);

            // First slot with token present is OK...
            List<ISlot> matchingSlot = new();
            // ...unless there are matching criteria specified in Settings class
            if (Settings.TokenSerial != null || Settings.TokenLabel != null)
            {
                matchingSlot = null;

                foreach (ISlot slot in slots)
                {
                    ITokenInfo tokenInfo = null;

                    try
                    {
                        tokenInfo = slot.GetTokenInfo();
                    }
                    catch (Pkcs11Exception ex)
                    {
                        if (ex.RV != CKR.CKR_TOKEN_NOT_RECOGNIZED && ex.RV != CKR.CKR_TOKEN_NOT_PRESENT)
                            throw;
                    }

                    if (tokenInfo == null)
                        continue;

                    if (!string.IsNullOrEmpty(Settings.TokenSerial))
                        if (0 != string.Compare(Settings.TokenSerial, tokenInfo.SerialNumber, StringComparison.Ordinal))
                            continue;

                    if (!string.IsNullOrEmpty(Settings.TokenLabel))
                        if (0 != string.Compare(Settings.TokenLabel, tokenInfo.Label, StringComparison.Ordinal))
                            continue;

                    matchingSlot.Add(slot);
                }
            }
            else matchingSlot = slots;

            //                Assert.IsTrue(matchingSlot != null, "Token matching criteria specified in Settings class is not present");
            return matchingSlot;
            }

            /// <summary>
            /// Creates the data object.
            /// </summary>
            /// <param name='session'>Read-write session with user logged in</param>
            /// <returns>Object handle</returns>
            public static IObjectHandle CreateDataObject(Net.Pkcs11Interop.HighLevelAPI.ISession session)
            {
                // Prepare attribute template of new data object
                List<IObjectAttribute> objectAttributes = new List<IObjectAttribute>();
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_DATA));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_APPLICATION, Settings.ApplicationName));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, Settings.ApplicationName));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_VALUE, "Data object content"));

                // Create object
                return session.CreateObject(objectAttributes);
            }

            /// <summary>
            /// Generates symetric key.
            /// </summary>
            /// <param name='session'>Read-write session with user logged in</param>
            /// <returns>Object handle</returns>
            public static IObjectHandle GenerateKey(Net.Pkcs11Interop.HighLevelAPI.ISession session)
            {
                // Prepare attribute template of new key
                List<IObjectAttribute> objectAttributes = new List<IObjectAttribute>();
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_SECRET_KEY));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_KEY_TYPE, CKK.CKK_DES3));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ENCRYPT, true));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_DECRYPT, true));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_DERIVE, true));
                objectAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_EXTRACTABLE, true));

                // Specify key generation mechanism
                IMechanism mechanism = session.Factories.MechanismFactory.Create(CKM.CKM_DES3_KEY_GEN);

                // Generate key
                return session.GenerateKey(mechanism, objectAttributes);
            }

            /// <summary>
            /// Generates asymetric key pair.
            /// </summary>
            /// <param name='session'>Read-write session with user logged in</param>
            /// <param name='publicKeyHandle'>Output parameter for public key object handle</param>
            /// <param name='privateKeyHandle'>Output parameter for private key object handle</param>
            public static void GenerateKeyPair(Net.Pkcs11Interop.HighLevelAPI.ISession session, out IObjectHandle publicKeyHandle, out IObjectHandle privateKeyHandle)
            {
                // The CKA_ID attribute is intended as a means of distinguishing multiple key pairs held by the same subject
                byte[] ckaId = session.GenerateRandom(20);

                // Prepare attribute template of new public key
                List<IObjectAttribute> publicKeyAttributes = new List<IObjectAttribute>();
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PRIVATE, false));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, Settings.ApplicationName));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ID, ckaId));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ENCRYPT, true));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_VERIFY, true));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_VERIFY_RECOVER, true));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_WRAP, true));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_MODULUS_BITS, 1024));
                publicKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PUBLIC_EXPONENT, new byte[] { 0x01, 0x00, 0x01 }));

                // Prepare attribute template of new private key
                List<IObjectAttribute> privateKeyAttributes = new List<IObjectAttribute>();
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PRIVATE, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, Settings.ApplicationName));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ID, ckaId));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_SENSITIVE, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_DECRYPT, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_SIGN, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_SIGN_RECOVER, true));
                privateKeyAttributes.Add(session.Factories.ObjectAttributeFactory.Create(CKA.CKA_UNWRAP, true));

                // Specify key generation mechanism
                IMechanism mechanism = session.Factories.MechanismFactory.Create(CKM.CKM_RSA_PKCS_KEY_PAIR_GEN);

                // Generate key pair
                session.GenerateKeyPair(mechanism, publicKeyAttributes, privateKeyAttributes, out publicKeyHandle, out privateKeyHandle);
            }
        }
    }