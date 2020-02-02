using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Connector.Models;
using MqttLib;

namespace Connector.Controllers
{
    public class MqttController : BaseController
    {
        IMqtt _client;

        // GET: Mqtt
        public ActionResult Index()
        {
            string retorno = string.Empty;
            ColetorTopicoLog ctlm = new ColetorTopicoLog();
            ctlm.Id_ColetorTopico = 1;
            ctlm.DataHora = getData();
            ctlm.Valor = "IndexInicio";
            db.ColetorTopicoLog.Add(ctlm);
            db.SaveChanges();

            try
            {
                Thread myNewThread = new Thread(() => Run());                
                myNewThread.Start();
                retorno = "OK";
            }
            catch (Exception erro)
            {
                retorno = erro.Message;
            }
            ViewBag.Retorno = retorno;
            return View();
        }

        void Run()
        {
            ColetorTopicoLog ctlm = new ColetorTopicoLog();
            ctlm.Id_ColetorTopico = 1;
            ctlm.DataHora = getData();
            ctlm.Valor = "RunInicio";
            db.ColetorTopicoLog.Add(ctlm);
            db.SaveChanges();

            /*
             client.username_pw_set("","ESQXpO-H7-1y")
            client.connect("m14.cloudmqtt.com", 11718, 60)
             
             */
            // string connectionString = "mqtt://m13.cloudmqtt.com:12644";
            string connectionString = "tcp://m14.cloudmqtt.com:11718";
            // Instantiate client using MqttClientFactory
            //_client = MqttClientFactory.CreateClient(connectionString, "aneuk", "clpfcosb", "ILo_4ucaK3P_");
            _client = MqttClientFactory.CreateClient(connectionString, "aneuk", "fgwuwgpw", "ESQXpO-H7-1y");
            // Setup some useful client delegate callbacks
            _client.Connected += new ConnectionDelegate(client_Connected);
            _client.ConnectionLost += new ConnectionDelegate(_client_ConnectionLost);
            _client.PublishArrived += new PublishArrivedDelegate(client_PublishArrived);
            //
            ColetorTopicoLog ctlm2 = new ColetorTopicoLog();
            ctlm2.Id_ColetorTopico = 1;
            ctlm2.DataHora = getData();
            ctlm2.Valor = "RunFim";
            db.ColetorTopicoLog.Add(ctlm2);
            db.SaveChanges();
            Start();
        }

        void Start()
        {
            //ColetorTopicoLog ctlm = new ColetorTopicoLog();
            //ctlm.Id_ColetorTopico = 1;
            //ctlm.DataHora = getData();
            //ctlm.Valor = "Start Inicio";
            //db.ColetorTopicoLog.Add(ctlm);
            //db.SaveChanges();

           // _client.Connect(true);
            //ColetorTopicoLog ctlm2 = new ColetorTopicoLog();
            //ctlm2.Id_ColetorTopico = 1;
            //ctlm2.DataHora = getData();
            //ctlm2.Valor = "Start Fim";
            //db.ColetorTopicoLog.Add(ctlm2);
            //db.SaveChanges();

            try
            {
                _client.Connect(true);
                //ColetorTopicoLog ctlm2 = new ColetorTopicoLog();
                //ctlm2.Id_ColetorTopico = 1;
                //ctlm2.DataHora = getData();
                //ctlm2.Valor = "Start Fim";
                //db.ColetorTopicoLog.Add(ctlm2);
                //db.SaveChanges();
            }
            catch (Exception exc)
            {

                ColetorTopicoLog ctlm2 = new ColetorTopicoLog();
                ctlm2.Id_ColetorTopico = 1;
                ctlm2.DataHora = getData();
                ctlm2.Valor = "Erro: " + exc.Message;
                db.ColetorTopicoLog.Add(ctlm2);
                db.SaveChanges();
            }
            // Connect to broker in 'CleanStart' mode
            //Console.WriteLine("Client connecting\n");
        }

        void Stop()
        {
            if (_client.IsConnected)
            {
                Console.WriteLine("Client disconnecting\n");
                _client.Disconnect();
                Console.WriteLine("Client disconnected\n");
            }
        }

        void client_Connected(object sender, EventArgs e)
        {
            ColetorTopicoLog ctlm2 = new ColetorTopicoLog();
            ctlm2.Id_ColetorTopico = 1;
            ctlm2.DataHora = getData();
            ctlm2.Valor = "Client Connected";
            db.ColetorTopicoLog.Add(ctlm2);
            db.SaveChanges();

            Console.WriteLine("Client connected\n");
            RegisterOurSubscriptions();
            PublishStart();
            //ColetorTopicoLog ctlm1 = new ColetorTopicoLog();
            //ctlm1.Id_ColetorTopico = 1;
            //ctlm1.DataHora = getData();
            //ctlm1.Valor = "Client Connect Fim";
            //db.ColetorTopicoLog.Add(ctlm1);
            //db.SaveChanges();
        }

        void _client_ConnectionLost(object sender, EventArgs e)
        {
            //Console.WriteLine("Client connection lost\n");
            ColetorTopicoLog ctlm = new ColetorTopicoLog();
            ctlm.Id_ColetorTopico = 1;
            ctlm.DataHora = getData();
            ctlm.Valor = "lost";
            db.ColetorTopicoLog.Add(ctlm);
            db.SaveChanges();

            DateTime dt = DateTime.Now;
            _client.Publish("Desconectando", "MQTT Web Server - " + dt.ToShortTimeString(), QoS.BestEfforts, false);
        }

        void RegisterOurSubscriptions()
        {
            //Console.WriteLine("Subscribing to mqttdotnet/subtest/#\n");
            _client.Subscribe("temperatura", QoS.BestEfforts);
            _client.Subscribe("pressao", QoS.BestEfforts);
        }

        void PublishStart()
        {
            //Console.WriteLine("Publishing on mqttdotnet/pubtest\n");
            DateTime dt = DateTime.Now;
            _client.Publish("Conectando", "MQTT Web Server - " + dt.ToShortTimeString(), QoS.BestEfforts, false);
        }

        bool client_PublishArrived(object sender, PublishArrivedArgs e)
        {
            ColetorTopicoLog ctlm = new ColetorTopicoLog();
            //
            if (e.Topic.Equals("temperatura"))
            {
                ctlm.Id_ColetorTopico = 1;
                ctlm.DataHora = getData();
                ctlm.Valor = e.Payload;
                //
                db.ColetorTopicoLog.Add(ctlm);
                db.SaveChanges();
            }
            else if (e.Topic.Equals("pressao"))
            {
                ctlm.Id_ColetorTopico = 2;
                ctlm.DataHora = getData();
                ctlm.Valor = e.Payload;
                //
                db.ColetorTopicoLog.Add(ctlm);
                db.SaveChanges();
            }
            //Console.WriteLine("Received Message");
            //Console.WriteLine("Topic: " + e.Topic);
            //Console.WriteLine("Payload: " + e.Payload);
            ////
            //Console.WriteLine();
            return true;
        }
    }
}