//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    
    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal unsafe struct cef_request_handler_t
    {
        internal cef_base_t _base;
        internal IntPtr _on_before_browse;
        internal IntPtr _on_before_resource_load;
        internal IntPtr _get_resource_handler;
        internal IntPtr _on_resource_redirect;
        internal IntPtr _get_auth_credentials;
        internal IntPtr _on_quota_request;
        internal IntPtr _on_protocol_execution;
        internal IntPtr _on_certificate_error;
        internal IntPtr _on_before_plugin_load;
        internal IntPtr _on_plugin_crashed;
        internal IntPtr _on_render_process_terminated;
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int add_ref_delegate(cef_request_handler_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int release_delegate(cef_request_handler_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int get_refct_delegate(cef_request_handler_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_before_browse_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request, int is_redirect);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_before_resource_load_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate cef_resource_handler_t* get_resource_handler_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_request_t* request);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_resource_redirect_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, cef_string_t* old_url, cef_string_t* new_url);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int get_auth_credentials_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_frame_t* frame, int isProxy, cef_string_t* host, int port, cef_string_t* realm, cef_string_t* scheme, cef_auth_callback_t* callback);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_quota_request_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* origin_url, long new_size, cef_quota_callback_t* callback);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_protocol_execution_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* url, int* allow_os_execution);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_certificate_error_delegate(cef_request_handler_t* self, CefErrorCode cert_error, cef_string_t* request_url, cef_allow_certificate_error_callback_t* callback);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int on_before_plugin_load_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* url, cef_string_t* policy_url, cef_web_plugin_info_t* info);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_plugin_crashed_delegate(cef_request_handler_t* self, cef_browser_t* browser, cef_string_t* plugin_path);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void on_render_process_terminated_delegate(cef_request_handler_t* self, cef_browser_t* browser, CefTerminationStatus status);
        
        private static int _sizeof;
        
        static cef_request_handler_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_request_handler_t));
        }
        
        internal static cef_request_handler_t* Alloc()
        {
            var ptr = (cef_request_handler_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_request_handler_t();
            ptr->_base._size = (UIntPtr)_sizeof;
            return ptr;
        }
        
        internal static void Free(cef_request_handler_t* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        
    }
}
