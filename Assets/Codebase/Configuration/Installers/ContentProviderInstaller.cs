using Codebase.Data;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ContentProviderInstaller", menuName = "Installers/ContentProviderInstaller")]
public class ContentProviderInstaller : ScriptableObjectInstaller<ContentProviderInstaller>
{
    public ContentProvider contentProvider;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ContentProvider>().FromInstance(contentProvider).AsSingle();
    }
}