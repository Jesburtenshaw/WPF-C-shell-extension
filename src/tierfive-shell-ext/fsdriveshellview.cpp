#include "pch.h"
#include "fsdriveshellview.h"

UINT CfsDriveShellView::sm_uListID = 101;

CfsDriveShellView::CfsDriveShellView()
	: m_uUIState(SVUIA_DEACTIVATE),
	m_hwndParent(nullptr),
	m_hMenu(nullptr),
	m_pContainingFolder(nullptr)
{
	LOGINFO(_MainApplication->GetLogger(), L"CfsDriveShellView constructor called");
}

CfsDriveShellView::~CfsDriveShellView()
{
	LOGINFO(_MainApplication->GetLogger(), L"CfsDriveShellView destructor called");
}

HRESULT CfsDriveShellView::FinalConstruct()
{
	LOGINFO(_MainApplication->GetLogger(), L"FinalConstruct called");
	return S_OK;
}

void CfsDriveShellView::FinalRelease()
{
	LOGINFO(_MainApplication->GetLogger(), L"FinalRelease called. Release the container folder");
	m_pContainingFolder.Release();
	m_pContainingFolder = nullptr;
}


IFACEMETHODIMP CfsDriveShellView::GetWindow(_Out_ HWND* phwnd)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Return the container window's handle to the browser");
		*phwnd = m_hWnd;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::ContextSensitiveHelp(_In_ BOOL fEnterMode)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"ContextSensitiveHelp: fEnterMode = %d", fEnterMode);
		LOGINFO(_MainApplication->GetLogger(), L"Doesn't support ��� ���� during an in-place activation session");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

HRESULT CallIDispatchMethod(IDispatch* pDisp, LPCWSTR name, CComVariant params[], int numParams, CComVariant& result)
{
	if (pDisp == NULL)
		return E_POINTER;
	DISPID dispId;
	LPOLESTR v1[] = { (LPOLESTR)name };
	HRESULT hr = pDisp->GetIDsOfNames(IID_NULL, v1, 1, LOCALE_SYSTEM_DEFAULT, &dispId);
	if (FAILED(hr))
	{
		return hr;
	}
	DISPPARAMS dp;
	dp.cNamedArgs = 0;
	dp.cArgs = numParams;
	dp.rgdispidNamedArgs = NULL;
	dp.rgvarg = params;
	EXCEPINFO exInfo;
	UINT n = 0;
	hr = pDisp->Invoke(dispId, IID_NULL, LOCALE_SYSTEM_DEFAULT, DISPATCH_METHOD, &dp, &result, &exInfo, &n);
	if (FAILED(hr))
	{
		return hr;
	}
	return S_OK;
}

void CfsDriveShellView::LoadCDM(HWND hWnd)
{
	m_pCLRLoader = std::make_unique<CCLRLoaderSimple>();
	HRESULT hr = m_pCLRLoader->CreateInstance(L"CDMWrapper", L"CDMWrapper.CDMWrapper", &m_cdmPtr);
	if (FAILED(hr))
	{
		return;
	}

	CComVariant v1((UINT64)hWnd);

	CComVariant params[]{ v1 }, result;
	hr = CallIDispatchMethod(m_cdmPtr, L"showCDM", params, 1, result);
	if (FAILED(hr))
	{
		return;
	}
}

IFACEMETHODIMP CfsDriveShellView::CreateViewWindow(_In_ LPSHELLVIEW pPrevious, _In_ LPCFOLDERSETTINGS pfs, _In_ LPSHELLBROWSER psb, _In_ LPRECT prcView, _Out_ HWND* phWnd)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Creates a custom shell view");
		*phWnd = NULL;

		LOGINFO(_MainApplication->GetLogger(), L"Init member variables");
		m_pShellBrowser = psb;
		m_FolderSettings = *pfs;

		LOGINFO(_MainApplication->GetLogger(), L"Get the parent window from Explorer");
		m_pShellBrowser->GetWindow(&m_hwndParent);
		LOGINFO(_MainApplication->GetLogger(), L"The parent window handle is 0x%08X", m_hwndParent);

		LOGINFO(_MainApplication->GetLogger(), L"Create a container window");
		if (NULL == Create(m_hwndParent, *prcView))
			return E_FAIL;

		LOGINFO(_MainApplication->GetLogger(), L"Return our window handle to the browser.");
		*phWnd = m_hWnd;
		LoadCDM(m_hWnd);
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::DestroyViewWindow()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Clean up the UI");
		UIActivate(SVUIA_DEACTIVATE);

		LOGINFO(_MainApplication->GetLogger(), L"Destroy the menu");
		if (NULL != m_hMenu)
			DestroyMenu(m_hMenu);

		LOGINFO(_MainApplication->GetLogger(), L"Destroy the window");
		DestroyWindow();
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::GetCurrentInfo(_Out_ LPFOLDERSETTINGS pfs)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"GetCurrentInfo:");
		*pfs = m_FolderSettings;
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::Refresh()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Refresh the window");
		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::UIActivate(_In_ UINT uState)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"UIActivate: uState = %d", uState);

		LOGINFO(_MainApplication->GetLogger(), L"Nothing to do if the state hasn't changed since the last call");
		if (m_uUIState == uState)
			return S_OK;

		LOGINFO(_MainApplication->GetLogger(), L"Modify the Explorer menu and status bar.");
		_HandleActivate(uState);

		return S_OK;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::AddPropertySheetPages(_In_ DWORD dwReserved, _In_ LPFNSVADDPROPSHEETPAGE pfn, _In_ LPARAM lparam)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"AddPropertySheetPages:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::EnableModeless(_In_ BOOL fEnable)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"EnableModeless: fEnable = %d", fEnable);
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::GetItemObject(_In_ UINT uItem, _In_ REFIID riid, _Out_ LPVOID* ppv)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"GetItemObject: uItem = %d", uItem);
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::SaveViewState()
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"SaveViewState:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::SelectItem(_In_ PCUITEMID_CHILD pidlItem, _In_ SVSIF uFlags)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"SelectItem:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::TranslateAccelerator(LPMSG msg)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"TranslateAccelerator:");
		return E_NOTIMPL;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::QueryStatus(_In_ const GUID* pguidCmdGroup, _In_ ULONG cCmds, _In_ OLECMD prgCmds[], _Out_ OLECMDTEXT* pCmdText)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"QueryStatus:");

		if (NULL == prgCmds)
			return E_POINTER;

		// The only useful standard command I've figured out is "refresh".  I've put some trace messages in so you can see the other commands that the
		// browser sends our way. 

		if (NULL == pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is NULL");

			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - DEFAULT: %u\n", prgCmds[u]);

				switch (prgCmds[u].cmdID)
				{
				case OLECMDID_REFRESH:
					prgCmds[u].cmdf = OLECMDF_SUPPORTED | OLECMDF_ENABLED;
					break;
				}
			}

			return S_OK;
		}
		else if (CGID_Explorer == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_Explorer");

			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - EXPLORER: %u\n", prgCmds[u]);
			}
		}
		else if (CGID_ShellDocView == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_ShellDocView");
			for (UINT u = 0; u < cCmds; u++)
			{
				LOGINFO(_MainApplication->GetLogger(), L"Query - DOCVIEW: %u\n", prgCmds[u]);
			}
		}

		return OLECMDERR_E_UNKNOWNGROUP;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

IFACEMETHODIMP CfsDriveShellView::Exec(_In_ const GUID* pguidCmdGroup, _In_ DWORD nCmdID, _In_ DWORD nCmdexecopt, _In_ VARIANT* pvaIn, _Out_ VARIANT* pvaOut)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Exec:");
		HRESULT hrRet = OLECMDERR_E_UNKNOWNGROUP;

		// The only standard command we act on is "refresh".  I've put some trace messages in so you can see the other commands that the
		// browser sends our way.

		if (NULL == pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is NULL");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - DEFAULT: %u", nCmdID);

			if (OLECMDID_REFRESH == nCmdID)
			{
				Refresh();
				hrRet = S_OK;
			}
		}
		else if (CGID_Explorer == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_Explorer");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - EXPLORER : %u", nCmdID);
		}
		else if (CGID_ShellDocView == *pguidCmdGroup)
		{
			LOGINFO(_MainApplication->GetLogger(), L"guidCmdGroup is CGID_ShellDocView");
			LOGINFO(_MainApplication->GetLogger(), L"Exec - DOCVIEW: %u", nCmdID);
		}

		return hrRet;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return E_FAIL;
	}
}

LRESULT CfsDriveShellView::OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"OnCreate:");
		HWND hHostWnd = _MainApplication->CreateViewHostInstance(m_hwndParent);

		if (hHostWnd != 0)
		{
			LOGINFO(_MainApplication->GetLogger(), L"lWnd is %x", hHostWnd);

			m_wndHost.Attach(hHostWnd);
			m_wndHost.SetParent(m_hWnd);
			if (!m_wndHost.IsWindowVisible())
			{
				m_wndHost.ShowWindow(SW_SHOW);
			}
		}

		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CfsDriveShellView::OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"Resize the list control to the same size as the container window.");

		if (m_wndHost.IsWindow())
			m_wndHost.MoveWindow(0, 0, LOWORD(lParam), HIWORD(lParam), TRUE);

		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CfsDriveShellView::OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"set the focus to the list control.");
		// This handler is called when the list container window gets the focus, 
		// usually because the user tabbed to it.  Immediately set the focus to
		// the list control.

		if (m_wndHost.IsWindow())
			m_wndHost.SetFocus();
		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

LRESULT CfsDriveShellView::OnContextMenu(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	try
	{
		LOGINFO(_MainApplication->GetLogger(), L"OnContextMenu:");
		return 0;
	}
	catch (...)
	{
		LOGERROR(_MainApplication->GetLogger(), L"The exception occured!");
		return (-1);
	}
}

void CfsDriveShellView::Init(CComPtr<IShellFolder>& pContainingFolder)
{
	m_pContainingFolder = pContainingFolder;
}


void CfsDriveShellView::_HandleActivate(UINT uState)
{
	LOGINFO(_MainApplication->GetLogger(), L"Undo our previous changes to the menu");
	_HandleDeactivate();

	LOGINFO(_MainApplication->GetLogger(), L"If we are being activated, add our stuff to Explorer's menu");
	if (SVUIA_DEACTIVATE != uState)
	{
		LOGINFO(_MainApplication->GetLogger(), L"create a new menu");

		LOGINFO(_MainApplication->GetLogger(), L"Modify the status bar");
		m_pShellBrowser->SetStatusTextSB(L"Assetmax CRM Namespace Extension");
	}

	LOGINFO(_MainApplication->GetLogger(), L"Save the current state");
	m_uUIState = uState;
}

void CfsDriveShellView::_HandleDeactivate()
{
	if (SVUIA_DEACTIVATE != m_uUIState)
	{
		LOGINFO(_MainApplication->GetLogger(), L"Deactivate menu and status bar");
		if (NULL != m_hMenu)
		{
			m_pShellBrowser->SetMenuSB(NULL, NULL, NULL);
			m_pShellBrowser->RemoveMenusSB(m_hMenu);

			LOGINFO(_MainApplication->GetLogger(), L"also destroys the SimpleNSExt submenu");
			DestroyMenu(m_hMenu);
			m_hMenu = NULL;
		}

		m_uUIState = SVUIA_DEACTIVATE;
	}
}