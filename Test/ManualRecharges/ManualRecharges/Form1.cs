using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace ManualRecharges
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            integraMobileWS.integraMobileWS oWS = new integraMobileWS.integraMobileWS();
            //oWS.Credentials =  new NetworkCredential("integraMobilePreProd","ZkDl:5cVyK");
            oWS.Credentials = new NetworkCredential("integraMobile", "$%&MiLR(=!");

            /*oWS.ManualRecharge("a.santosaudera@gmail.com", 1000);
            oWS.ManualRecharge("agm_jce@hotmail.com", 500);
            oWS.ManualRecharge("alarcon_mendoza@yahoo.es", 1000);
            oWS.ManualRecharge("alejandra.jardon@gmail.com", 2000);
            oWS.ManualRecharge("alxmorrison@hotmail.com", 1000);
            oWS.ManualRecharge("ángel@offsetti.com", 1000);
            oWS.ManualRecharge("azuparedes@gmail.com", 2000);
            oWS.ManualRecharge("belencasajuana@gmail.com", 1000);
            oWS.ManualRecharge("belensm@gmail.com", 50);
            oWS.ManualRecharge("blanca.de.las.rivas@gmail.com", 1000);

            oWS.ManualRecharge("borja_porcar@msn.com", 50);
            oWS.ManualRecharge("borjacameron@gmail.com", 1000);
            oWS.ManualRecharge("carlopaniagua@gmail.com", 2000);
            oWS.ManualRecharge("carlos.landecho@yahoo.es", 2000);
            oWS.ManualRecharge("carmen.delrio@uralita.com", 2000);
            oWS.ManualRecharge("casarecas@gmail.com", 1000);
            oWS.ManualRecharge("cesar777p@hotmail.com", 500);
            oWS.ManualRecharge("cesarcollazosbueno@gmail.com", 3000);
            oWS.ManualRecharge("chacon.luis@gmail.com", 2000);
            oWS.ManualRecharge("crisullastres@hotmail.com", 50);
            oWS.ManualRecharge("dagobhaa@hotmail.com", 2000);
            oWS.ManualRecharge("danisirol@hotmail.com", 1000);
            oWS.ManualRecharge("dedomps@yahoo.es", 2000);
            oWS.ManualRecharge("edeleyva@yahoo.es", 1000);
            oWS.ManualRecharge("eduardo.barrosom@gmail.com", 1000);
            oWS.ManualRecharge("edumartmend@gmail.com", 500);
            oWS.ManualRecharge("elpeter@gmail.com", 1000);
            oWS.ManualRecharge("estefania@bsi.com.es", 1000);
            oWS.ManualRecharge("ezequielgonzalez80@gmail.com", 5000);
            oWS.ManualRecharge("farmaciachaconh@gmail.com", 500);
            oWS.ManualRecharge("fblanch@gmail.com", 50);
            oWS.ManualRecharge("fernanda.matoses@deoleo.eu", 2000);
            oWS.ManualRecharge("fernando.moreno@arrakis.es", 1000);
            oWS.ManualRecharge("francoisepialoux@mac.com", 1000);
            oWS.ManualRecharge("gab1975@gmail.com", 500);
            oWS.ManualRecharge("gonzalezchimeno@hotmail.com", 1000);
            oWS.ManualRecharge("guillermocanals@yahoo.es", 1000);
            oWS.ManualRecharge("ibon_pivoli@hotmail.com", 500);
            oWS.ManualRecharge("info@soundtelco.com", 2000);
            oWS.ManualRecharge("ipardilla@alarde.es", 2000);
            oWS.ManualRecharge("isaroblesrod@gmail.com", 1000);
            oWS.ManualRecharge("j.baronprieto@gmail.com", 2000);
            oWS.ManualRecharge("jacosta@gmail.com", 1000);
            oWS.ManualRecharge("jaimebarez@gmail.com", 2000);
            oWS.ManualRecharge("javier.escolano2296@gmail.com", 2000);
            oWS.ManualRecharge("javier.fjqp@gmail.com", 1000);
            oWS.ManualRecharge("javierdpablo@hotmail.com", 2000);
            oWS.ManualRecharge("jcalvarezrio@gmail.com", 1000);
            oWS.ManualRecharge("jdelatorresb@gmail.com", 500);
            oWS.ManualRecharge("jesus.esteban@avcast.es", 2000);
            oWS.ManualRecharge("jesus.llorente.martinez@gmail.com", 2000);
            oWS.ManualRecharge("jgarbayo@acker.es", 1000);
            oWS.ManualRecharge("jmguilleuma@europautomotive.net", 2000);
            oWS.ManualRecharge("jmoreno@sytesa.com", 1000);
            oWS.ManualRecharge("joacus@gmail.com", 2000);
            oWS.ManualRecharge("joaquin.lopez.arribas@gmail.com", 2000);
            oWS.ManualRecharge("jorgederamosd@gmail.com", 1000);
            oWS.ManualRecharge("jorgehr40@gmail.com", 2000);
            oWS.ManualRecharge("juanantonio@balbaran.es", 1000);
            oWS.ManualRecharge("julgarlo@yahoo.es", 5000);
            oWS.ManualRecharge("julio.romero@benow.es", 50);
            oWS.ManualRecharge("lesoson@gmail.com", 1000);
            oWS.ManualRecharge("lmontoliu@yahoo.es", 1000);
            oWS.ManualRecharge("luciaybarra@gmail.com", 2000);
            oWS.ManualRecharge("manel.castro@mantequeriasarias.com", 500);
            oWS.ManualRecharge("marabierto_8@hotmail.com", 500);
            oWS.ManualRecharge("marioavila@terra.com", 500);
            oWS.ManualRecharge("martinezramos.daniel@gmail.com", 500);
            oWS.ManualRecharge("medico@cliniteq.com", 2000);
            oWS.ManualRecharge("meriadocbrandigamo@hotmail.com", 500);
            oWS.ManualRecharge("merigpy@gmail.com", 1100);
            oWS.ManualRecharge("mgil@reformaspergola.com", 1000);
            oWS.ManualRecharge("migueldej66@gmail.com", 1000);
            oWS.ManualRecharge("mjbarbudo@telefonica.net", 3000);
            oWS.ManualRecharge("natalia.viguri@gmail.com", 1000);
            oWS.ManualRecharge("neilmam@yahoo.com", 2000);
            oWS.ManualRecharge("nievesmanzano@hotmail.com", 2000);
            oWS.ManualRecharge("nrocha@consorseguros.es", 1000);
            oWS.ManualRecharge("otrahistoria@mac.com", 2000);
            oWS.ManualRecharge("patriciaallona@gmail.com", 2000);
            oWS.ManualRecharge("pecado@pecadofilms.com", 1000);
            oWS.ManualRecharge("phelyks@hotmail.com", 2000);
            oWS.ManualRecharge("rachelarista@hotmail.com", 1000);
            oWS.ManualRecharge("ramirez.elia@gmail.com", 2000);
            oWS.ManualRecharge("realdeasua.teresa@gmail.com", 500);
            oWS.ManualRecharge("rgonzalez@altamarcapital.com", 2000);
            oWS.ManualRecharge("rocioquirantes@gmail.com", 1000);
            oWS.ManualRecharge("rs_cg@hotmail.com", 1000);
            oWS.ManualRecharge("silvia.susaeta@gmail.com", 2000);
            oWS.ManualRecharge("susana.alvaro@ono.com", 1000);
            oWS.ManualRecharge("susana.navarro.serrano@gmail.com", 1000);
            oWS.ManualRecharge("tizqui@hotmail.com", 1000);
            oWS.ManualRecharge("vazquezgmi@gmail.com", 1000);
            oWS.ManualRecharge("wallace.jw@gmail.com", 1000);
            oWS.ManualRecharge("yadi130@gmail.com", 1000);
             * 
             * */

            oWS.ManualRecharge("borjacameron@gmail.com", 1000);
            oWS.ManualRecharge("carlos.landecho@yahoo.es", 2000);
            oWS.ManualRecharge("cesar777p@hotmail.com", 500);
            oWS.ManualRecharge("cesarcollazosbueno@gmail.com", 3000);
            oWS.ManualRecharge("dagobhaa@hotmail.com", 2000);
            oWS.ManualRecharge("dedomps@yahoo.es", 2000);
            oWS.ManualRecharge("elpeter@gmail.com", 1000);
            oWS.ManualRecharge("farmaciachaconh@gmail.com", 500);
            oWS.ManualRecharge("fblanch@gmail.com", 50);
            oWS.ManualRecharge("francoisepialoux@mac.com", 1000);
            oWS.ManualRecharge("gab1975@gmail.com", 500);
            oWS.ManualRecharge("jesus.esteban@avcast.es", 2000);
            oWS.ManualRecharge("joacus@gmail.com", 2000);
            oWS.ManualRecharge("jorgederamosd@gmail.com", 1000);
            oWS.ManualRecharge("juanantonio@balbaran.es", 1000);
            oWS.ManualRecharge("manel.castro@mantequeriasarias.com", 500);
            oWS.ManualRecharge("marabierto_8@hotmail.com", 500);
            oWS.ManualRecharge("migueldej66@gmail.com", 1000);
            oWS.ManualRecharge("nrocha@consorseguros.es", 1000);
            oWS.ManualRecharge("patriciaallona@gmail.com", 2000);
            oWS.ManualRecharge("ramirez.elia@gmail.com", 2000);
            oWS.ManualRecharge("wallace.jw@gmail.com", 1000);
            oWS.ManualRecharge("yadi130@gmail.com", 1000);

























        }
    }
}
