using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Convenctions
{
    public class CustomControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            //controller.ControllerName....; //внесение изменений в то, как система будет взаимодействовать с контроллерами
        }
    }
}
