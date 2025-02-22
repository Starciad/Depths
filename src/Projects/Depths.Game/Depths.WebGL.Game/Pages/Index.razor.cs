using Microsoft.JSInterop;

using Depths.Core;

using System.Threading.Tasks;

namespace Depths.Game.Pages
{
    public partial class Index
    {
        private DGame _game;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await this.JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            // init game
            if (this._game == null)
            {
                this._game = new();
                this._game.Run();
            }

            // run gameloop
            this._game.Tick();
        }
    }
}
