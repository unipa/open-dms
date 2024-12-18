namespace OpenDMS.A3Synch.API.Utility
{
    public static class SharedVariables
    {
        public static DateTime startSynch { set; get; }
        public static string kc_access_token {set; get; }
        public static string kc_refresh_token { set; get; }
        public static int kc_roles_elaborated { set; get; }
        public static int kc_roles_total { set; get; }
        public static int kc_users_elaborated { set; get; }
        public static int kc_users_total { set; get; }
        public static bool isSyncing { set; get; }
        public static int elaborated_contacts_counter { set; get; }
        public static int total_contacts_counter { set; get; }
        public static int elaborated_nodes_counter { set; get; }
        public static int total_nodes_counter { set; get; }
        public static int elaborated_roles_counter { set; get; }
        public static int total_roles_counter { set; get; }
        public static int elaborated_usergroupsroles_counter { set; get; }
        public static int total_usergroupsroles_counter { set; get; }
        public static int elaborated_users_counter { set; get; }
        public static int total_users_counter { set; get; }
        public static int total_groups_counter { set; get; }
        public static int elaborated_groups_counter { set; get; }
        public static int elaborated_keycloak_users_counter { set; get; }
        public static int total_keycloak_users_counter { set; get; }
        public static int total_addedRole_counter { set; get; }
        public static int elaborated_addedRole_counter { set; get; }
        public static int elaborated_member_list { set; get; }
        public static int total_addedIdp_counter { set; get; }
        public static int elaborated_addedIdp_counter { set; get; }
        public static string? synch_error { set; get; }
        public static string? synch_error_UserGroups { set; get; }
        public static string? synch_error_Organigramma { set; get; }
        public static string? synch_error_RoleInDb { set; get; }
        public static string? synch_error_ContactsInDb { set; get; }
        public static string? synch_error_UsersInDb { set; get; }
        public static string? synch_error_UserGroupsRolesInDb { set; get; }
        public static string? synch_error_SynchAllInKC { set; get; }

    }
}
