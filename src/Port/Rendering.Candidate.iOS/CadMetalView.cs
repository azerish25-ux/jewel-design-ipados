using CoreGraphics;
using Foundation;
using Metal;
using MetalKit;
using UIKit;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;

/// <summary>Visible, on-demand CAD rendering with retained uploads and bounded asynchronous frames.</summary>
[Register("JDCadMetalView")]
public sealed class CadMetalView : MTKView
{
    private readonly CadMetalDrawablePresenter presenter;
    private readonly ViewDelegate drawDelegate;
    private volatile bool viewDisposed;
    private Exception? viewError;
    private float yaw=.6f,pitch=.35f;
    public event Action<CadMetalFrameInfo>? FramePresented;
    public event Action<CadMetalFrameInfo>? FrameGpuCompleted;
    public event Action<Exception>? FrameFailed;
    public CadMetalViewStats Stats{get{var snapshot=presenter.Stats;return snapshot with{LastError=snapshot.LastError??viewError?.ToString()};}}

    private static IMTLDevice MainThreadDevice()
    {
        RequireMainThread();return MTLDevice.SystemDefault??throw new NotSupportedException("Metal device is unavailable.");
    }
    private static void RequireMainThread()
    {if(!NSThread.IsMain)throw new InvalidOperationException("Viewport mutations require the main UI thread.");}
    public CadMetalView(CGRect frame):base(frame,MainThreadDevice())
    {
        presenter=new();
        ColorPixelFormat=MTLPixelFormat.BGRA8Unorm;DepthStencilPixelFormat=MTLPixelFormat.Depth32Float;SampleCount=1;
        ClearColor=CadMetalRenderer.Background;ClearDepth=1;FramebufferOnly=true;
        AutoResizeDrawable=true;Paused=true;EnableSetNeedsDisplay=true;
        var weak=new WeakReference<CadMetalView>(this);
        presenter.Presented=info=>Post(weak,view=>view.FramePresented?.Invoke(info));
        presenter.GpuCompleted=info=>Post(weak,view=>view.FrameGpuCompleted?.Invoke(info));
        presenter.Failed=error=>Post(weak,view=>view.Fail(error));
        presenter.Completed=()=>Post(weak,view=>{if(view.presenter.CanSubmit)view.SetNeedsDisplay();});
        drawDelegate=new ViewDelegate(weak);Delegate=drawDelegate;
    }
    private static void Post(WeakReference<CadMetalView> owner,Action<CadMetalView> action)
    {
        UIApplication.SharedApplication.BeginInvokeOnMainThread(()=>{if(owner.TryGetTarget(out var view)&&!view.viewDisposed)action(view);});
    }
    public long SetMesh(CadViewportMesh mesh)
    {
        RequireMainThread();ObjectDisposedException.ThrowIf(viewDisposed,this);
        long id=presenter.SetMesh(mesh);SetNeedsDisplay();return id;
    }
    public void SetCameraPose(float yaw,float pitch)
    {
        RequireMainThread();ObjectDisposedException.ThrowIf(viewDisposed,this);
        _=CadViewportCamera.Fit(1,1,yaw,pitch);
        if(this.yaw==yaw&&this.pitch==pitch)return;
        this.yaw=yaw;this.pitch=pitch;presenter.Invalidate();SetNeedsDisplay();
    }
    private void Resize()
    {
        if(viewDisposed)return;RequireMainThread();presenter.Invalidate();SetNeedsDisplay();
    }
    private void DrawFrame()
    {
        if(viewDisposed||!presenter.CanSubmit)return;
        RequireMainThread();
        var drawable=CurrentDrawable;if(drawable is null)return;
        bool transferred=false;
        try
        {
            using var pass=CurrentRenderPassDescriptor;if(pass is null)return;
            int width=checked((int)drawable.Texture.Width),height=checked((int)drawable.Texture.Height);
            if(width==0||height==0)return;
            pass.ColorAttachments[0].LoadAction=MTLLoadAction.Clear;pass.ColorAttachments[0].StoreAction=MTLStoreAction.Store;pass.ColorAttachments[0].ClearColor=CadMetalRenderer.Background;
            var depth=pass.DepthAttachment??throw new InvalidOperationException("Visible depth attachment unavailable.");
            depth.LoadAction=MTLLoadAction.Clear;depth.StoreAction=MTLStoreAction.DontCare;depth.ClearDepth=1;
            transferred=presenter.TrySubmit(pass,drawable,CadViewportCamera.Fit(width,height,yaw,pitch),width,height);
        }
        catch(Exception error){Fail(error);}
        finally{if(!transferred)drawable.Dispose();}
    }
    private void Fail(Exception error){viewError??=error;FrameFailed?.Invoke(error);}
    protected override void Dispose(bool disposing)
    {
        if(!viewDisposed)
        {
            viewDisposed=true;presenter?.Dispose();
            if(disposing){Delegate=null;drawDelegate?.Dispose();}
        }
        base.Dispose(disposing);
    }
    private sealed class ViewDelegate(WeakReference<CadMetalView> owner):MTKViewDelegate
    {
        public override void Draw(MTKView view){if(owner.TryGetTarget(out var target))target.DrawFrame();}
        public override void DrawableSizeWillChange(MTKView view,CGSize size){if(owner.TryGetTarget(out var target))target.Resize();}
    }
}
