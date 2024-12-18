namespace Web.Model.Customize
{
    public class FirmeViewModel
    {
        public string RemoteSignService { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string Icon { get; set; }
        public bool CanHaveHandwrittenSignature { get; set; }
        public bool CanHaveRemoteSignature { get; set; }
    }
}
