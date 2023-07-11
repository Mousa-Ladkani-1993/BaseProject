public class _RolesNames
{
    public _RolesNames() {}
    private _RolesNames(string value) { this.value = value; }

    public string value { get; private set;}

    
    public override string ToString() 
    {
        return this.value;
    }

    public static bool operator == (_RolesNames r, string s)
    {
        return r.value == s;
    }

    public static bool operator != (_RolesNames r, string s)
    {
        return r.value != s;
    }

    public static bool operator == (_RolesNames r1, _RolesNames r2)
    {
        return r1.value == r2.value;
    }

    public static bool operator != (_RolesNames r1, _RolesNames r2)
    {
        return r1.value != r2.value;
    }
    
    public static _RolesNames TEXT_VARIABLES { get => new _RolesNames("TEXT VARIABLES"); }
    public static _RolesNames LOOKUPS { get => new _RolesNames("LOOKUPS"); }
    public static _RolesNames INQUIRIES { get => new _RolesNames("INQUIRIES"); }
    public static _RolesNames COMMUNITIES { get => new _RolesNames("COMMUNITIES"); }
    public static _RolesNames MEDIA_FILES { get => new _RolesNames("MEDIA FILES"); }
    public static _RolesNames SLIDERS { get => new _RolesNames("SLIDERS"); }
    public static _RolesNames CONTENT_RECORDS { get => new _RolesNames("CONTENT RECORDS"); }
    public static _RolesNames COMPANY_LOOKUPS { get => new _RolesNames("COMPANY LOOKUPS"); }
    public static _RolesNames USERS { get => new _RolesNames("USERS"); }
    public static _RolesNames ALBUMS { get => new _RolesNames("ALBUMS"); }
    public static _RolesNames FAQS { get => new _RolesNames("FAQS"); }
    public static _RolesNames SYSTEM_PARAMETERS { get => new _RolesNames("SYSTEM PARAMETERS"); }
    public static _RolesNames AUTHORS { get => new _RolesNames("AUTHORS"); }
    public static _RolesNames COUNTRIES { get => new _RolesNames("COUNTRIES"); }
    public static _RolesNames KEYWORDS { get => new _RolesNames("KEYWORDS"); }
    public static _RolesNames ROLES { get => new _RolesNames("ROLES"); }
    public static _RolesNames CAREERS { get => new _RolesNames("CAREERS"); }
    public static _RolesNames MENUS_AND_PAGES { get => new _RolesNames("MENUS AND PAGES"); }
    public static _RolesNames PUSH_NOTIFICATION { get => new _RolesNames("PUSH NOTIFICATION"); }
}