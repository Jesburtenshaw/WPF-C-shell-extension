

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Tue Jan 19 11:14:07 2038
 */
/* Compiler settings for CDMshellext.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.01.0628 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __CDMshellext_i_h__
#define __CDMshellext_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

#ifndef DECLSPEC_XFGVIRT
#if defined(_CONTROL_FLOW_GUARD_XFG)
#define DECLSPEC_XFGVIRT(base, func) __declspec(xfg_virtual(base, func))
#else
#define DECLSPEC_XFGVIRT(base, func)
#endif
#endif

/* Forward Declarations */ 

#ifndef __IFSShellPlugin_FWD_DEFINED__
#define __IFSShellPlugin_FWD_DEFINED__
typedef interface IFSShellPlugin IFSShellPlugin;

#endif 	/* __IFSShellPlugin_FWD_DEFINED__ */


#ifndef __ICDMShellExt_FWD_DEFINED__
#define __ICDMShellExt_FWD_DEFINED__
typedef interface ICDMShellExt ICDMShellExt;

#endif 	/* __ICDMShellExt_FWD_DEFINED__ */


#ifndef __ICDMShellView_FWD_DEFINED__
#define __ICDMShellView_FWD_DEFINED__
typedef interface ICDMShellView ICDMShellView;

#endif 	/* __ICDMShellView_FWD_DEFINED__ */


#ifndef __CDMShellExt_FWD_DEFINED__
#define __CDMShellExt_FWD_DEFINED__

#ifdef __cplusplus
typedef class CDMShellExt CDMShellExt;
#else
typedef struct CDMShellExt CDMShellExt;
#endif /* __cplusplus */

#endif 	/* __CDMShellExt_FWD_DEFINED__ */


#ifndef __CDMShellView_FWD_DEFINED__
#define __CDMShellView_FWD_DEFINED__

#ifdef __cplusplus
typedef class CDMShellView CDMShellView;
#else
typedef struct CDMShellView CDMShellView;
#endif /* __cplusplus */

#endif 	/* __CDMShellView_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IFSShellPlugin_INTERFACE_DEFINED__
#define __IFSShellPlugin_INTERFACE_DEFINED__

/* interface IFSShellPlugin */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IFSShellPlugin;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("AC80FE12-31FD-4374-9E71-A6ED7D605FA0")
    IFSShellPlugin : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Done( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRootWindow( 
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IFSShellPluginVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IFSShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfoCount)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IFSShellPlugin * This,
            /* [out] */ UINT *pctinfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfo)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IFSShellPlugin * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetIDsOfNames)
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IFSShellPlugin * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        DECLSPEC_XFGVIRT(IDispatch, Invoke)
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IFSShellPlugin * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, Init)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, Done)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Done )( 
            IFSShellPlugin * This);
        
        DECLSPEC_XFGVIRT(IFSShellPlugin, GetRootWindow)
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRootWindow )( 
            IFSShellPlugin * This,
            /* [in] */ HWND hwndOwner,
            /* [retval][out] */ HWND *phwnd);
        
        END_INTERFACE
    } IFSShellPluginVtbl;

    interface IFSShellPlugin
    {
        CONST_VTBL struct IFSShellPluginVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IFSShellPlugin_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IFSShellPlugin_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IFSShellPlugin_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IFSShellPlugin_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IFSShellPlugin_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IFSShellPlugin_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IFSShellPlugin_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IFSShellPlugin_Init(This)	\
    ( (This)->lpVtbl -> Init(This) ) 

#define IFSShellPlugin_Done(This)	\
    ( (This)->lpVtbl -> Done(This) ) 

#define IFSShellPlugin_GetRootWindow(This,hwndOwner,phwnd)	\
    ( (This)->lpVtbl -> GetRootWindow(This,hwndOwner,phwnd) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IFSShellPlugin_INTERFACE_DEFINED__ */


#ifndef __ICDMShellExt_INTERFACE_DEFINED__
#define __ICDMShellExt_INTERFACE_DEFINED__

/* interface ICDMShellExt */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICDMShellExt;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("b203cd4b-9f89-4949-9aa2-8ffe7ac02de9")
    ICDMShellExt : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICDMShellExtVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICDMShellExt * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICDMShellExt * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICDMShellExt * This);
        
        END_INTERFACE
    } ICDMShellExtVtbl;

    interface ICDMShellExt
    {
        CONST_VTBL struct ICDMShellExtVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICDMShellExt_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICDMShellExt_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICDMShellExt_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICDMShellExt_INTERFACE_DEFINED__ */


#ifndef __ICDMShellView_INTERFACE_DEFINED__
#define __ICDMShellView_INTERFACE_DEFINED__

/* interface ICDMShellView */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_ICDMShellView;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("252BA8FF-F9DC-4758-B72C-17C05D53BC72")
    ICDMShellView : public IUnknown
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ICDMShellViewVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICDMShellView * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICDMShellView * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICDMShellView * This);
        
        END_INTERFACE
    } ICDMShellViewVtbl;

    interface ICDMShellView
    {
        CONST_VTBL struct ICDMShellViewVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICDMShellView_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICDMShellView_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICDMShellView_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICDMShellView_INTERFACE_DEFINED__ */



#ifndef __CDMshellextLib_LIBRARY_DEFINED__
#define __CDMshellextLib_LIBRARY_DEFINED__

/* library CDMshellextLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_CDMshellextLib;

EXTERN_C const CLSID CLSID_CDMShellExt;

#ifdef __cplusplus

class DECLSPEC_UUID("f70a8770-1f4b-4af5-90e4-35260bcd97df")
CDMShellExt;
#endif

EXTERN_C const CLSID CLSID_CDMShellView;

#ifdef __cplusplus

class DECLSPEC_UUID("61963ADC-3165-404C-9381-32C21CD3C754")
CDMShellView;
#endif
#endif /* __CDMshellextLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  HWND_UserSize(     unsigned long *, unsigned long            , HWND * ); 
unsigned char * __RPC_USER  HWND_UserMarshal(  unsigned long *, unsigned char *, HWND * ); 
unsigned char * __RPC_USER  HWND_UserUnmarshal(unsigned long *, unsigned char *, HWND * ); 
void                      __RPC_USER  HWND_UserFree(     unsigned long *, HWND * ); 

unsigned long             __RPC_USER  HWND_UserSize64(     unsigned long *, unsigned long            , HWND * ); 
unsigned char * __RPC_USER  HWND_UserMarshal64(  unsigned long *, unsigned char *, HWND * ); 
unsigned char * __RPC_USER  HWND_UserUnmarshal64(unsigned long *, unsigned char *, HWND * ); 
void                      __RPC_USER  HWND_UserFree64(     unsigned long *, HWND * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


