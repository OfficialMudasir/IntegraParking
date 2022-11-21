using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateOrUpdateMembership
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            integraMobileWS.integraMobileWS oWS = new integraMobileWS.integraMobileWS();
            //oWS.Credentials = new System.Net.NetworkCredential("integraMobile", "$%&MiLR(=!"); //eysamobile
            oWS.Credentials = new System.Net.NetworkCredential("integraMobile", "kI5~6.!5_j"); //iparkme
            
            if (oWS.CreateOrUpdateMembership(txtUsername.Text, txtEmail.Text, txtPassword.Text))
            {
                MessageBox.Show("Success!!!!!!");
            }
            else
            {
                MessageBox.Show("Error!!!!!!");
            }

            /*oWS.CreateOrUpdateMembership("jomaboes@gmail.com", "jomaboes@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ceciegamazo@gmail.com", "ceciegamazo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ndsystema@ndsystema.com", "ndsystema@ndsystema.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rdezal@gmail.com", "rdezal@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("bluis70@gmail.com", "bluis70@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("delhoyo@hotmail.com", "delhoyo@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("menalfonso@gmail.com", "menalfonso@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("santiago@hinves.com", "santiago@hinves.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mbcontrerasolmedo@gmail.com", "mbcontrerasolmedo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("menalfonso1@gmail.com", "menalfonso1@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("enrique@gantiq.com", "enrique@gantiq.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("intalacon@gmail.com", "intalacon@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jesuscerrajero@hotmail.com", "jesuscerrajero@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cangaj@gmail.com", "cangaj@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("laurita4ever_@hotmail.com", "laurita4ever_@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rgarciarivas@gmail.com", "rgarciarivas@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("antonio.avto@gmail.com", "antonio.avto@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("pilaracaltapia@gmail.com", "pilaracaltapia@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cespinosahg@gmail.com", "cespinosahg@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("isaromeroca@gmail.com", "isaromeroca@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("amarperitaciones@yahoo.es", "amarperitaciones@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("maria.vareladelimia@telefonica.com", "maria.vareladelimia@telefonica.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("irenemartin22@gmail.com", "irenemartin22@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("myriamcreus@gmail.com", "myriamcreus@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("evelyn_r2006@yahoo.es", "evelyn_r2006@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ricardovila@mipropiaweb.com", "ricardovila@mipropiaweb.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mrv.gryphus@gmail.com", "mrv.gryphus@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("peacita7@gmail.com", "peacita7@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rojoalcubo3@gmail.com", "rojoalcubo3@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("joseraul.arroyo@gmail.com", "joseraul.arroyo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jmartinezm@ciccp.es", "jmartinezm@ciccp.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jcmartinez@gruporegio.com", "jcmartinez@gruporegio.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("esthernalda@icar.es", "esthernalda@icar.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("esthercurry@movistar.es", "esthercurry@movistar.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eishe9@hotmail.com", "eishe9@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("carmen.prieto82@gmail.com", "carmen.prieto82@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("miraquetelodije@hotmail.com", "miraquetelodije@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("maria.luz.amusategui@gmail.com", "maria.luz.amusategui@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jose.antonio.de.manuel@gmail.com", "jose.antonio.de.manuel@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("anaisabelredondo@gmail.com", "anaisabelredondo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("carlgadi@gmail.com", "carlgadi@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("belentelva@gmail.com", "belentelva@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("javierdealbaf@gmail.com", "javierdealbaf@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("alejandro2005@gmail.com", "alejandro2005@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("chuchorocket@hotmail.com", "chuchorocket@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("tsolsona.c@gmail.com", "tsolsona.c@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("tiomkin@eresmas.com", "tiomkin@eresmas.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("psalascarbajosa@gmail.com", "psalascarbajosa@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("camilork@gmail.com", "camilork@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jaimeolivadefuentes@gmail.com", "jaimeolivadefuentes@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("manacu@ubu.es", "manacu@ubu.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juan@cloudonmobile.com", "juan@cloudonmobile.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("santanam.alberto@gmail.com", "santanam.alberto@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jelenagomez@gmail.com", "jelenagomez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mariogcanedo@gmail.com", "mariogcanedo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("talleresbugatti@hotmail.com", "talleresbugatti@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sergioanton@yahoo.com", "sergioanton@yahoo.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sariita.87@gmail.com", "sariita.87@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rub1978@gmail.com", "rub1978@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("fcanop@inicia.es", "fcanop@inicia.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("pabloquintian@gmail.com", "pabloquintian@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ljnievesc@gmail.com", "ljnievesc@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juanjolopezs@yahoo.com", "juanjolopezs@yahoo.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("buzkall@gmail.com", "buzkall@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("henio2001@yahoo.com", "henio2001@yahoo.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("comunica@mapo.es", "comunica@mapo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("fran201283@hotmail.es", "fran201283@hotmail.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sirvichispi@yahoo.es", "sirvichispi@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gonzibest@hotmail.com", "gonzibest@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("nube1900@gmail.com", "nube1900@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ikerpescador@hotmail.com", "ikerpescador@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jmletona@escuelapensamientomatematico.es", "jmletona@escuelapensamientomatematico.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("virginoia@yahoo.es", "virginoia@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("iker.pescador@constructorasanjose.com", "iker.pescador@constructorasanjose.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eva.g.596@gmail.com", "eva.g.596@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("lauraasensiolopez@gmail.com", "lauraasensiolopez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rafa@estudiocaco.com", "rafa@estudiocaco.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("josecarlos.diaz@bq.com", "josecarlos.diaz@bq.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("djlegolas@hotmail.com", "djlegolas@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mariacanocadahia@gmail.com", "mariacanocadahia@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("operaciones@fiscourier.com", "operaciones@fiscourier.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("j.eslava.f@gmail.com", "j.eslava.f@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("enrique.travesi@hotmail.com", "enrique.travesi@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eduherraiz@gmail.com", "eduherraiz@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gonaranguren@gmail.com", "gonaranguren@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gisellaguerzoni@gmail.com", "gisellaguerzoni@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("xavisequier@gmail.com", "xavisequier@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("estelarodriguezv@gmail.com", "estelarodriguezv@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("josecarlos.diaz@bqreaders.com", "josecarlos.diaz@bqreaders.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("pertigaza@gmail.com", "pertigaza@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("javiersantamaria72@yahoo.es", "javiersantamaria72@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("chuscr2@gmail.com", "chuscr2@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("simoburghi@gmail.com", "simoburghi@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("arnalcasanova@gmail.com", "arnalcasanova@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("bsotill@yahoo.es", "bsotill@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gberecob@gmail.com", "gberecob@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gilgon51@hotmail.com", "gilgon51@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("guillermo@montefilm.es", "guillermo@montefilm.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("norman_lmb@hotmail.com", "norman_lmb@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("monicagomezgarcia@gmail.com", "monicagomezgarcia@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rociomontoiro@hotmail.com", "rociomontoiro@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("uit.honduras@hotmail.com", "uit.honduras@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gusanita1971@gmail.com", "gusanita1971@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("anely1@hotmail.com", "anely1@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ivonne.salazar2010@gmail.com", "ivonne.salazar2010@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jlopezov@hotmail.com", "jlopezov@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("karinagarciabecerra@gmail.com", "karinagarciabecerra@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sara__mg@hotmail.com", "sara__mg@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("joconnop@gmail.com", "joconnop@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("carmenmgarciar@gmail.com", "carmenmgarciar@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("anabelen.saiz@gmail.com", "anabelen.saiz@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("nuevoana@gmail.com", "nuevoana@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("joconnorp@gmail.com", "joconnorp@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("toteinusa@hotmail.com", "toteinusa@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cazalla86@hotmail.com", "cazalla86@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("charoferrerm@hotmail.com", "charoferrerm@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mruigomez@bufeteruigomez.es", "mruigomez@bufeteruigomez.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mamerinero79@gmail.com", "mamerinero79@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gabriel.elorriaga@hotmail.com", "gabriel.elorriaga@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jrgc74@gmail.com", "jrgc74@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("igalindo@kpmg.es", "igalindo@kpmg.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rachacon@cisco.com", "rachacon@cisco.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juanvallet@vallet-abogados.es", "juanvallet@vallet-abogados.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("joseberja@hotmail.com", "joseberja@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("isaac.sanz@prosegur.com", "isaac.sanz@prosegur.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("c.fernandezespinosa@gmail.com", "c.fernandezespinosa@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("pacolatorre@bluewin.ch", "pacolatorre@bluewin.ch", txtPassword.Text);
            oWS.CreateOrUpdateMembership("josecasamayor@me.com", "josecasamayor@me.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("belmon.pro@gmail.com", "belmon.pro@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juliusmore@hotmail.com", "juliusmore@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juanmaoboe@gmail.com", "juanmaoboe@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("neville.isaac@gmail.com", "neville.isaac@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("goyete@gmail.com", "goyete@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eloyasenjo@telefonica.net", "eloyasenjo@telefonica.net", txtPassword.Text);
            oWS.CreateOrUpdateMembership("llm102@hotmail.com", "llm102@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ragazzino22@live.com", "ragazzino22@live.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cye.psicologia@gmail.com", "cye.psicologia@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jlbarquin@gmail.com", "jlbarquin@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("luisa.delatorre@gmail.com", "luisa.delatorre@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("motiloa@hotmail.com", "motiloa@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("picado_rios@yahoo.es", "picado_rios@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cmainar@gmail.com", "cmainar@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("diego.corregidor@hotmail.com", "diego.corregidor@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("andresgarciasuarez@gmail.com", "andresgarciasuarez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("quisaba@yahoo.es", "quisaba@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("m.serrano.izarra@gmail.com", "m.serrano.izarra@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("vanelo@gmail.com", "vanelo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gvisol@yahoo.es", "gvisol@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sartinano@arpo.es", "sartinano@arpo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("aliciasd87@gmail.com", "aliciasd87@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("carola081@hotmail.com", "carola081@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("dulcemari_72@hotmail.com", "dulcemari_72@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jarechab@hotmail.com", "jarechab@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cvalera@prisatv.com", "cvalera@prisatv.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("marina.sette@gmail.com", "marina.sette@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("silvia.perez.alvarez@gmail.com", "silvia.perez.alvarez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("j.antonio.tomas@gmail.com", "j.antonio.tomas@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rodrigo@eloyserigrafia.es", "rodrigo@eloyserigrafia.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("rakelnuniezfc@gmail.com", "rakelnuniezfc@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("monikemarin@gmail.com", "monikemarin@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jllarsan@gmail.com", "jllarsan@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("santimoreno223@gmail.com", "santimoreno223@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("carlosrello@hotmail.com", "carlosrello@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("amartinmedina@gmail.com", "amartinmedina@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("anarpal@hotmail.com", "anarpal@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("saramartinez.pelaez@gmail.com", "saramartinez.pelaez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("nbedmar@gmail.com", "nbedmar@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cgmaroto@yahoo.es", "cgmaroto@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("nachete.duran@gmail.com", "nachete.duran@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("salvajesteatro@gmail.com", "salvajesteatro@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cosinjalm@hotmail.com", "cosinjalm@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("javier.munoz.pereira@hotmail.com", "javier.munoz.pereira@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ignacio.martinez.ortiz@es.pwc.com", "ignacio.martinez.ortiz@es.pwc.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("friogallo@hotmail.com", "friogallo@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("silvia_garvar@hotmail.com", "silvia_garvar@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("lav2612@gmail.com", "lav2612@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cristinabalag1@gmail.com", "cristinabalag1@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("juliogp2600@gmail.com", "juliogp2600@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("martadelvalleinclan@hotmail.com", "martadelvalleinclan@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("lfromojaro@hotmail.com", "lfromojaro@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sendazul@gmail.com", "sendazul@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("santiagoglez@orange.es", "santiagoglez@orange.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("conchi.moraleda@gmail.com", "conchi.moraleda@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("trillejo@gmail.com", "trillejo@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ramfistrujillo@yahoo.es", "ramfistrujillo@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("saritta_18@hotmail.com", "saritta_18@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mariano.garcia@clh.com", "mariano.garcia@clh.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("atejedorlopez@gmail.com", "atejedorlopez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("alex.garcia.nicolas@gmail.com", "alex.garcia.nicolas@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jgutierrezo@live.com", "jgutierrezo@live.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("arvilla09@gmail.com", "arvilla09@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("fdelcampog@yahoo.es", "fdelcampog@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("benuuu_2007@yahoo.com", "benuuu_2007@yahoo.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gus.garcia@yahoo.es", "gus.garcia@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("fernando.f.lazaro@gmail.com", "fernando.f.lazaro@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eloi_gas@hotmail.com", "eloi_gas@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("claudemil_8@hotmail.com", "claudemil_8@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("terrece@gmail.com", "terrece@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jmdaused@aena.es", "jmdaused@aena.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("beatrizezponda@hotmail.com", "beatrizezponda@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("borja.ogando@gmail.com", "borja.ogando@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("eysa@mcm.pm", "eysa@mcm.pm", txtPassword.Text);
            oWS.CreateOrUpdateMembership("igorprieto@hotmail.com", "igorprieto@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jordiriber@gmail.com", "jordiriber@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gonzaleztorres.francisco@gmail.com", "gonzaleztorres.francisco@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ayaesj@hotmail.es", "ayaesj@hotmail.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("avelino.gomez@roche.com", "avelino.gomez@roche.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("judithbermejo@yahoo.es", "judithbermejo@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("sylviakballero@gmail.com", "sylviakballero@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("mfuentest@afuera.com", "mfuentest@afuera.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jose_botella@europe.bd.com", "jose_botella@europe.bd.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("gel.sanchez@hotmail.com", "gel.sanchez@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("javiertapia.arte@gmail.com", "javiertapia.arte@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("velilla@aparte.es", "velilla@aparte.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("reyesgamo2000@yahoo.es", "reyesgamo2000@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("nikolinaapostolova@gmail.com", "nikolinaapostolova@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("s.rn2005@hotmail.com", "s.rn2005@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("egm@acacias25.com", "egm@acacias25.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("daviddepedro@ono.com", "daviddepedro@ono.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("grasu_elena23tsk@yahoo.es", "grasu_elena23tsk@yahoo.es", txtPassword.Text);
            oWS.CreateOrUpdateMembership("andrea.marin.marco@gmail.com", "andrea.marin.marco@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("hugolozanogarcia@gmail.com", "hugolozanogarcia@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("webestival@hotmail.com", "webestival@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("martabaezam@gmail.com", "martabaezam@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("javibruce@gmail.com", "javibruce@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("migueldefernandez@gmail.com", "migueldefernandez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("trinidefernandez@gmail.com", "trinidefernandez@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("jj@keide.com", "jj@keide.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("anitaizqui@hotmail.com", "anitaizqui@hotmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("ljrmc8@gmail.com", "ljrmc8@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("m.bacaicoa72@gmail.com", "m.bacaicoa72@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("cbarcellos13@gmail.com", "cbarcellos13@gmail.com", txtPassword.Text);
            oWS.CreateOrUpdateMembership("alexcacharron@hotmail.com", "alexcacharron@hotmail.com", txtPassword.Text);*/


        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            txtUsername.Text = txtEmail.Text;
        }
    }
}
