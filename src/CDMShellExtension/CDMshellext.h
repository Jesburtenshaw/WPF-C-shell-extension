// CDMshellext.h : Declaration of the CDMshellext

#pragma once
#include "resource.h"       // main symbols
#include "CDMshellext_i.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif


// CDMShellExt

class ATL_NO_VTABLE CDMShellExt :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CDMShellExt, &CLSID_CDMShellExt>,
	public IObjectWithSiteImpl<CDMShellExt>,
	public IPersistFolder2,
	public IShellFolder2
{
public:
	CDMShellExt();
	~CDMShellExt();

DECLARE_REGISTRY_RESOURCEID(IDR_CDMSHELLEXT)

BEGIN_COM_MAP(CDMShellExt)
	COM_INTERFACE_ENTRY2(IPersist, IPersistFolder2)
	COM_INTERFACE_ENTRY2(IPersistFolder, IPersistFolder2)
	COM_INTERFACE_ENTRY(IPersistFolder2)
	COM_INTERFACE_ENTRY2(IShellFolder, IShellFolder2)
	COM_INTERFACE_ENTRY(IShellFolder2)
END_COM_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct();
	void FinalRelease();

public:

	// IPersist
	IFACEMETHODIMP GetClassID(__out CLSID* pclsid);

	// IPersistFolder
	IFACEMETHODIMP Initialize(PCIDLIST_ABSOLUTE pidl);

	// IPersistFolder2
	IFACEMETHODIMP GetCurFolder(__out PIDLIST_ABSOLUTE* ppidl);

	// IShellFolder
	IFACEMETHODIMP ParseDisplayName(_In_ HWND hwnd, _In_ IBindCtx* pbc, _In_ PWSTR pszDisplayName, __inout ULONG* pchEaten, __deref_out PIDLIST_RELATIVE* ppidl, __inout ULONG* pdwAttributes);
	IFACEMETHODIMP EnumObjects(HWND hwnd, SHCONTF grfFlags, __deref_out IEnumIDList** ppenmIDList);
	IFACEMETHODIMP BindToObject(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv);
	IFACEMETHODIMP BindToStorage(PCUIDLIST_RELATIVE pidl, __in IBindCtx* pbc, REFIID riid, __deref_out void** ppv);
	IFACEMETHODIMP CompareIDs(LPARAM lParam, PCUIDLIST_RELATIVE pidl1, PCUIDLIST_RELATIVE pidl2);
	IFACEMETHODIMP CreateViewObject(HWND hwndOwner, REFIID riid, __deref_out void** ppv);
	IFACEMETHODIMP GetAttributesOf(UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, __inout SFGAOF* rgfInOut);
	IFACEMETHODIMP GetUIObjectOf(HWND hwndOwner, UINT cidl, __in_ecount_opt(cidl) PCUITEMID_CHILD_ARRAY rgpidl, REFIID riid, __reserved UINT* rgfReserved, __deref_out void** ppv);
	IFACEMETHODIMP GetDisplayNameOf(PCUITEMID_CHILD pidl, SHGDNF uFlags, __out STRRET* psrName);
	IFACEMETHODIMP SetNameOf(HWND hwnd, PCUITEMID_CHILD pidl, LPCWSTR pszName, SHGDNF uFlags, __deref_out_opt PITEMID_CHILD* ppidlOut);

	// IShellFolder2
	IFACEMETHODIMP GetDefaultSearchGUID(__out GUID* pguid);
	IFACEMETHODIMP EnumSearches(__deref_out IEnumExtraSearch** ppenum);
	IFACEMETHODIMP GetDefaultColumn(DWORD dwRes, __out ULONG* plSort, __out ULONG* plDisplay);
	IFACEMETHODIMP GetDefaultColumnState(UINT iColumn, __out SHCOLSTATEF* pcsFlags);
	IFACEMETHODIMP GetDetailsEx(PCUITEMID_CHILD pidl, const PROPERTYKEY* pkey, __out VARIANT* pvar);
	IFACEMETHODIMP GetDetailsOf(__in_opt PCUITEMID_CHILD pidl, UINT iColumn, __out SHELLDETAILS* psd);
	IFACEMETHODIMP MapColumnToSCID(UINT iColumn, __out PROPERTYKEY* pkey);

private:
	static const struct DISPLAYNAMEOFINFO
	{
		enum INDEX
		{
			GDNI_RELATIVEFRIENDLY = 0,
			GDNI_ABSOLUTEFRIENDLY,
			GDNI_RELATIVEPARSING,
			GDNI_ABSOLUTEPARSING
		};

		enum MASK
		{
			GDNM_INFOLDER = 0x00000001,
			GDNM_FORADDRESSBAR = 0x00000002,
			GDNM_FORPARSING = 0x00000004,
			GDNM_FOREDITING = 0x00000008
		};

		HRESULT(CDMShellExt::* _GetDisplayNameOf)(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath);
	} _DisplayNameOfInfo[];

	HRESULT _GetDisplayNameOfDisplayName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName);
	HRESULT _GetDisplayNameOfDisplayPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath);
	HRESULT _GetDisplayNameOfParsingName(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszName);
	HRESULT _GetDisplayNameOfParsingPath(PCUITEMID_CHILD pidl, SHGDNF uFlags, __deref_out PWSTR* ppszPath);
};

OBJECT_ENTRY_AUTO(__uuidof(CDMShellExt), CDMShellExt)
