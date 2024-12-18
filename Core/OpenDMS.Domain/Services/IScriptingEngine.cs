namespace OpenDMS.Domain.Services;
public interface IScriptingEngine
{
//    public Task<string> FileParse(string Text);
    public Task<string> Parse(string Text);

}