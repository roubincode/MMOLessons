
/// <summary>
/// 音频,图片等资源工厂的接口
/// </summary>
/// <typeparam name="T">区分不同种类资源的泛型</typeparam>
public interface IBaseResourceFactory<T>
{
    T GetSingleResources(string resourcePath);
}
