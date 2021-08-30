using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoodStore.Startup))]
namespace FoodStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

    //Users
    //user33@mvcfoodstore.com - Password@33
    //user12@foodstore.com - Password@01
}
