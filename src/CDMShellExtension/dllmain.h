// dllmain.h : Declaration of module class.

class CDMshellextModule : public ATL::CAtlDllModuleT< CDMshellextModule >
{
public :
	DECLARE_LIBID(LIBID_CDMshellextLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CDM_SHELL_EXT, "{09614f9a-0000-4e87-89e9-873111e4597a}")
};

extern class CDMshellextModule _AtlModule;
