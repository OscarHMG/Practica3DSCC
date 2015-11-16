using System;
using Microsoft.SPOT;
using GTM = Gadgeteer.Modules;
using GT = Gadgeteer;

namespace Practica3DSCC
{
    // Referencia tipo "delegate" para función callback ObjectOn
    public delegate void ObjectOnEventHandler();

    // Referencia tipo "delegate" para función callback ObjectOff
    public delegate void ObjectOffEventHandler();

    /*
     * Clase SensorProximidad, encapsula el funcionanmiento del sensor de proximidad infrarrojo.
     * Esta clase gestiona los dos componentes del sensor: el LED infrarrojo y el foto-transistor.
     * Además, dispara dos eventos: ObjectOn y ObjectOff cuando el sensor detecta la presencia o
     * ausencia de un objeto.
     */
    class SensorProximidad
    {
        //EVENTO ObjectOff: Disparar este evento cuando el sensor detecte la ausencia del objeto
        public event ObjectOffEventHandler ObjectOff;
        Boolean singleton = false;
        //EVENTO ObjectOn: Disparar este evento cuando el sensor detecte la presencia de un objeto
        public event ObjectOnEventHandler ObjectOn;
        private GT.Timer timer;
        private GT.SocketInterfaces.AnalogInput input;
        private GT.SocketInterfaces.DigitalOutput output;
        private static double voltage;
        
        public SensorProximidad(GTM.GHIElectronics.Extender extender)
        {
            //TODO: Inicializar el sensor
            input = extender.CreateAnalogInput(GT.Socket.Pin.Three); 
            output = extender.CreateDigitalOutput(GT.Socket.Pin.Five, false);
            timer = new GT.Timer(1000);
            timer.Tick += new GT.Timer.TickEventHandler(timer_Tick);
            //timer.Start();

              
        }



        private void timer_Tick(GT.Timer timer)
        {
            Debug.Print("voltage: " +input.ReadVoltage());
            voltage = input.ReadVoltage();
            if(voltage<=3.3 && voltage>=3) //false
            {
                if (!singleton) { 
                    ObjectOff();
                    Debug.Print("OBJETO FUERA");
                    singleton=true;
                }
            }
            else //true
            {
                if (singleton)
                {
                    ObjectOn();
                    Debug.Print("OBJETO PRESENTE");
                    singleton = false;
                }
                
            }
        }

        public void StartSampling()
        {
            //TODO: Activar el LED infrarrojo y empezar a muestrear el foto-transistor
            output.Write(false);
            timer.Start();
            
            
        }

        public void StopSampling()
        {
            //TODO: Desactivar el LED infrarrojo y detener el muestreo del foto-transistor
            output.Write(true);
            timer.Stop();
            
            
        }


    }
}
