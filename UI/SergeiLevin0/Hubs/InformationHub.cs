using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SergeiLevin0.Hubs
{
    public class InformationHub: Hub
    {
        public async Task Send(string Message) => 
            await Clients.All.SendAsync("Send", Message);//берет всех клиентов зарегистрировавшихся в хабе, т.е. подключившихся к нему и отправляет им всем данные.
        //"Send" - метод отправки
        //в браузере при этом нужно подписаться к Хабу и указать в качестве действия, которое слушаем Send и этим же действием отправляем сообщение клиентам
    }
}
