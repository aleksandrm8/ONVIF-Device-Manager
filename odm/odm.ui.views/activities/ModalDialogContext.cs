using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using utils;
using System.Windows.Media;

namespace odm.ui.activities {
	public class ModalDialogContext:IUnityContainer, IDisposable {
		UnityContainer container = new UnityContainer();
		//protected Dispatcher dispatcher;
		//protected ModalDialogView dialog = new ModalDialogView();
		protected CompositeDisposable disposables = new CompositeDisposable();

		public ModalDialogContext(string title = null) {
			var activeView = new SerialDisposable();
			ModalDialogView dialog = null;// new ModalDialogView();
			var dispatcher = Application.Current.Dispatcher;
			//var dispatcher = dialog.Dispatcher;
			dispatcher.BeginInvoke(() => {
				dialog = new ModalDialogView();
				if (!disposables.IsDisposed) {
					disposables.Add(
						Disposable.Create(
							() => dispatcher.BeginInvoke(
								()=>dialog.Close()
							)
						)
					);
					dialog.Header = title;
					dialog.Owner = Application.Current.MainWindow;
					dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					dialog.SizeToContent = SizeToContent.WidthAndHeight;
					dialog.ShowInTaskbar = false;
					dialog.ShowDialog();
					activeView.Dispose();
				}
			});
			var presenter = ViewPresenter.Create(view => {
				var disp = new CompositeDisposable();
				activeView.Disposable = disp;
				dispatcher.BeginInvoke(()=>{
					dbg.Assert(dialog != null);
					if (!disp.IsDisposed) {
						dbg.Assert(dialog.Content == null);
						dialog.Content = view;
						var header = NavigationContext.GetTitle(view);
						dialog.Header = header ?? title;
						disp.Add( Disposable.Create(() => {
							dispatcher.BeginInvoke(() => {
								dbg.Assert(dialog.Content == view);
								dialog.Header = title;
								dialog.Content = null;
								var d = view as IDisposable;
								if (d != null) {
									d.Dispose();
								}
							});
						}));
					}
				});
				return disp;
			});
			container.RegisterInstance<IViewPresenter>(presenter);
		}

		#region IUnityContainer implementation
		public IUnityContainer AddExtension(UnityContainerExtension extension) {
			return container.AddExtension(extension);
		}

		public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides) {
			return container.BuildUp(t, existing, name, resolverOverrides);
		}

		public object Configure(Type configurationInterface) {
			return container.Configure(configurationInterface);
		}

		public IUnityContainer CreateChildContainer() {
			return container.CreateChildContainer();
		}

		public IUnityContainer Parent {
			get { return container.Parent; }
		}

		public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime) {
			return container.RegisterInstance(t, name, instance, lifetime);
		}

		public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) {
			return container.RegisterType(from, to, name, lifetimeManager, injectionMembers);
		}

		public IEnumerable<ContainerRegistration> Registrations {
			get { return container.Registrations;}
		}

		public IUnityContainer RemoveAllExtensions() {
			return container.RemoveAllExtensions();
		}

		public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides) {
			return container.Resolve(t, name, resolverOverrides);
		}

		public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides) {
			return container.ResolveAll(t, resolverOverrides);
		}

		public void Teardown(object o) {
			container.Teardown(o);
		}

		#endregion

		public void Dispose() {
			disposables.Dispose();
			container.Dispose();
		}
	}
	
}
