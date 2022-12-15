namespace RepositoryFramework.Web.Components
{
    public sealed class AppPalette
    {
        public string Primary { get; set; } = "#ff6d41";
        public string PrimaryLight { get; set; } = "#ff7f58";
        public string PrimaryLighter { get; set; } = "rgba(255, 109, 65, 0.16)";
        public string PrimaryDark { get; set; } = "#eb643c";
        public string PrimaryDarker { get; set; } = "#bf5231";
        public string Secondary { get; set; } = "#35a0d7";
        public string SecondaryLight { get; set; } = "#4dabdc";
        public string SecondaryLighter { get; set; } = "rgba(53, 160, 215, 0.2)";
        public string SecondaryDark { get; set; } = "#3193c6";
        public string SecondaryDarker { get; set; } = "#2878a1";
        public string Base50 { get; set; } = "#ffffff";
        public string Base100 { get; set; } = "#f6f7fa";
        public string Base200 { get; set; } = "#e9edf0";
        public string Base300 { get; set; } = "#c1c9cb";
        public string Base400 { get; set; } = "#95a4a8";
        public string Base500 { get; set; } = "#77858b";
        public string Base600 { get; set; } = "#dadfe2";
        public string Base700 { get; set; } = "#545e61";
        public string Base800 { get; set; } = "#3a474d";
        public string Base900 { get; set; } = "#28363c";
        public string Series1 { get; set; } = "#0479cc";
        public string Series2 { get; set; } = "#68d5c8";
        public string Series3 { get; set; } = "#ff6d41";
        public string Series4 { get; set; } = "#cb6992";
        public string Series5 { get; set; } = "#e6c54f";
        public string Series6 { get; set; } = "#f9777f";
        public string Series7 { get; set; } = "#5dbf74";
        public string Series8 { get; set; } = "#4db9f2";
        public void SetRedBase()
        {
            Base50 = "#ff0000";
            Base100 = "#eb1a1a";
            Base200 = "#db1a1a";
            Base300 = "#cb1a1a";
            Base400 = "#bf2b2b";
            Base500 = "#a53939";
            Base600 = "#9c4747";
            Base700 = "#7c4747";
            Base800 = "#6c4747";
            Base900 = "#5c1717";
        }
    }
}
