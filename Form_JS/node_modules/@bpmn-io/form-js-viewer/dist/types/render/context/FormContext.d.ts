export default FormContext;
declare const FormContext: import("preact").Context<{
    getService: typeof getService;
    formId: any;
}>;
/**
 * @param {string} type
 * @param {boolean} [strict]
 *
 * @returns {any}
 */
declare function getService(type: string, strict?: boolean): any;
