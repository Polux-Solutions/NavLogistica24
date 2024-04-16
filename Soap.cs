using NavLogistica24.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace NavLogistica24
{
    public class Soap
    {
        public async Task<string> Proveedores(mDatos xDatos, mProveedor xProvedor, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                XmlDocument soapEnvelopeXml = new XmlDocument();

                soapEnvelopeXml.LoadXml($@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent= ""http://entrada.webservice.inlog.inlogconsulting.es""> 
                                            <soapenv:Header />
                                            <soapenv:Body>
                                            <ent:callProveedor soapenv:encodingStyle= ""http://schemas.xmlsoap.org/soap/encoding/"">
                                                <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                                       <dateTime xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</dateTime>
                                                       <originatorName xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</originatorName>
                                                    </messageInfo>
                                                    <trxId xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xId}</trxId>
                                                 </in0>
                                                <in1 xsi:type= ""bean:Proveedor"" xmlns:bean= ""http://beans.webservice.inlogconsulting.com.es"">
                                                    <accion xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.accion}</accion>
                                                    <codext xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.codext}</codext>
                                                    <codpos xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.codpos}</codpos>
                                                    <copais xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.codpais}</copais>
                                                    <direcc xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.direcc}</direcc>
                                                    <fecha xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.fecha}</fecha>
                                                    <locali xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.locali}</locali>
                                                    <nombre xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.nombre}</nombre>
                                                    <nomcto xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.nomcto}</nomcto>
                                                    <numfax xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.numfax}</numfax>
                                                    <provin xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.provin}</provin>
                                                    <razsoc xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.razsoc}</razsoc>
                                                    <telefo xsi:type= ""soapenc:string"" xmlns:soapenc= ""http://schemas.xmlsoap.org/soap/encoding/"">{xProvedor.telefo}</telefo>
                                                </in1>
                                            </ent:callProveedor>
                                            </soapenv:Body>
                                            </soapenv:Envelope>");

                f.Guardar_xml(xDatos, "Proveedores", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos, "ProveedorWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }

            return Respuesta;
        }

        public async Task<string> Articulos(mDatos xDatos, mArticulo xArticulo, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                XmlDocument soapEnvelopeXml = new XmlDocument();

                string xml = $@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent= ""http://entrada.webservice.inlog.inlogconsulting.es""> 
                                <soapenv:Header />
                                <soapenv:Body>
                                <ent:callArticulo soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                    <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                        <messageInfo xsi:type=""bean:MessageInfo"">
                                            <dateTime xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</dateTime>
                                            <originatorName xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</originatorName>
                                        </messageInfo>
                                        <trxId xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xId}</trxId>
                                        </in0>
                                        <in1 xsi:type=""bean:Articulo"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                        <accion xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.accion}</accion>
                                        <almace xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.almace}</almace>
                                        <altcaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.altcaj}</altcaj>
                                        <altpal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.altpal}</altpal>
                                        <anccaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.anccaj}</anccaj>
                                        <ancpal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.ancpal}</ancpal>
                                        <artpro xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.artpro}</artpro>
                                        <artpv1 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.artpv1}</artpv1>
                                        <artpv2 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.artpv2}</artpv2>
                                        <artpvl xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.artpvl}</artpvl>
                                        <caddev xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.caddev}</caddev>
                                        <cajpal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.cajpal}</cajpal>
                                        <capkud xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.capkud}</capkud>
                                        <claabc xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.claabc}</claabc>";

                if (xArticulo.clacad != "") xml += $@"<clacad xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.clacad}</clacad>";

                xml += $@"              <denomi xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.denomi}</denomi>
                                        <ean128 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.ean128}</ean128>
                                        <fecha  xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.fecha}</fecha>
                                        <forven xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.forven}</forven>
                                        <gestns xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.gestns}</gestns>
                                        <grprod xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.grprod}</grprod>
                                        <idun14 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.idun14}</idun14>
                                        <iean13 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.iean13}</iean13>
                                        <indcad xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.indcad}</indcad>
                                        <indpes xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.indpes}</indpes>
                                        <indsus xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.indsus}</indsus>
                                        <inpesa xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.inpesa}</inpesa>
                                        <insren xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.insren}</insren>
                                        <insrsa xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.insrsa}</insrsa>
                                        <larcaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.larcaj}</larcaj>
                                        <loseob xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.loseob}</loseob>
                                        <loteob xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.loteob}</loteob>
                                        <lotpic xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.lotpic}</lotpic>
                                        <lotsal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.lotsal}</lotsal>
                                        <manpal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.manpal}</manpal>
                                        <percua xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.percua}</percua>
                                        <pescaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.pescaj}</pescaj>
                                        <pesvar xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.pesvar}</pesvar>
                                        <propie xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.propie}</propie>
                                        <sitart xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.sitart}</sitart>
                                        <tipart xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.tipart}</tipart>
                                        <tipent xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.tipent}</tipent>
                                        <tipeti xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.tipeti}</tipeti>
                                        <tippal xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.tippal}</tippal>
                                        <tolera xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.tolera}</tolera>
                                        <unicaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.unicaj}</unicaj>
                                        <unidns xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.unidns}</unidns>
                                        <unimed xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.unimed}</unimed>
                                        <valdec xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.valdec}</valdec>
                                        <vlogev xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.vlogev}</vlogev>
                                        <volcaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xArticulo.volcaj}</volcaj>
                                        </in1>
                                    </ent:callArticulo>
                                </soapenv:Body>
                                </soapenv:Envelope>";

                    soapEnvelopeXml.LoadXml(xml);

                f.Guardar_xml(xDatos, "Articulos", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos,"ArticuloWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        public async Task<string> Codigos_Barras(mDatos xDatos, mCodigoBarras xBarras, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                XmlDocument soapEnvelopeXml = new XmlDocument();

                soapEnvelopeXml.LoadXml($@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent= ""http://entrada.webservice.inlog.inlogconsulting.es""> 
                                            <soapenv:Header />
                                            <soapenv:Body>
                                              <ent:callCodigoBarras soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                                 <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                                       <dateTime xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</dateTime>
                                                       <originatorName xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</originatorName>
                                                    </messageInfo>
                                                    <trxId xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xId}</trxId>
                                                 </in0>
                                                 <in1 xsi:type=""bean:CodigoBarras"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                                    <accion xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.accion}</accion>
                                                    <almace xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.almace}</almace>
                                                    <artpro xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.artpro}</artpro>
                                                    <artpv1 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.artpv1}</artpv1>
                                                    <artpv2 xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.artpv2}</artpv2>
                                                    <artpvl xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.artpvl}</artpvl>
                                                    <codbar xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.codbar}</codbar>
                                                    <fecalt xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.fecalt}</fecalt>
                                                    <fecha xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.fecha}</fecha>
                                                    <propie xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.propie}</propie>
                                                    <tipcod xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.tipcod}</tipcod>
                                                    <unicaj xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xBarras.unicaj}</unicaj>
                                                 </in1>
                                              </ent:callCodigoBarras>
                                            </soapenv:Body>
                                            </soapenv:Envelope>");

                f.Guardar_xml(xDatos, "CodigosBarras", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos, "CodigoBarrasWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        public async Task<string> Clientes(mDatos xDatos, mCliente xCliente, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                XmlDocument soapEnvelopeXml = new XmlDocument();

                soapEnvelopeXml.LoadXml($@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent= ""http://entrada.webservice.inlog.inlogconsulting.es""> 
                                            <soapenv:Header />
                                              <soapenv:Body>
                                               <ent:callCliente soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                                 <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                                       <dateTime xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</dateTime>
                                                       <originatorName xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">?</originatorName>
                                                    </messageInfo>
                                                    <trxId xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xId}</trxId>
                                                 </in0>
                                                 <in1 xsi:type=""bean:Cliente"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                                    <accion xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.accion}</accion>
                                                    <codext xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.codext}</codext>
                                                    <codpos xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.codpos}</codpos>
                                                    <copais xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.copais}</copais>
                                                    <direcc xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.direcc}</direcc>
                                                    <fecha xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.fecha}</fecha>
                                                    <locali xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.locali}</locali>
                                                    <nombre xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.nombre}</nombre>
                                                    <nomcto xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.nomcto}</nomcto>
                                                    <numfax xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.numfax}</numfax>
                                                    <percon xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.percon}</percon>
                                                    <provin xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.provin}</provin>
                                                    <razsoc xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.razsoc}</razsoc>
                                                    <telefo xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.telefo}</telefo>
                                                    <tipcli xsi:type=""soapenc:string"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">{xCliente.tipcli}</tipcli>
                                                 </in1>
                                              </ent:callCliente>
                                             </soapenv:Body>
                                            </soapenv:Envelope>");

                    f.Guardar_xml(xDatos, "Clientes", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                    Respuesta = await GetRequest(xDatos, "ClienteWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        public async Task<string> OC(mDatos xDatos, mOCCabecera xOC, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                string xmlLineas = "";
                string xmlPedido = "";
                XmlDocument soapEnvelopeXml = new XmlDocument();


                for (int i=0; i<xOC.Lineas.Count; i++)
                {
                    mOCLineas iLinea = xOC.Lineas[i];

                    xmlLineas += $@"<LineaPedidoEntrada>
                                    <accion xsi:type=""soapenc:string"">{iLinea.accion}</accion>
                                    <artpro xsi:type=""soapenc:string"">{iLinea.artpro}</artpro>
                                    <artpv1 xsi:type=""soapenc:string"">{iLinea.artpv1}</artpv1>
                                    <artpv2 xsi:type=""soapenc:string"">{iLinea.artpv2}</artpv2>
                                    <artpvl xsi:type=""soapenc:string"">{iLinea.artpvl}</artpvl>
                                    <canteo xsi:type=""soapenc:string"">{iLinea.canteo}</canteo>
                                    <codlin xsi:type=""soapenc:string"">{iLinea.codlin}</codlin>
                                    <fecha xsi:type=""soapenc:string"">{iLinea.fecha}</fecha>
                                    <pedext xsi:type=""soapenc:string"">{iLinea.pedext}</pedext>
                                    <sitlin xsi:type=""soapenc:string"">{iLinea.sitlin}</sitlin>
                                    <tipfor xsi:type=""soapenc:string"">{iLinea.tipfor}</tipfor>
                                    <unicaj xsi:type=""soapenc:string"">{iLinea.unicaj}</unicaj>";

                    if (! string.IsNullOrEmpty(iLinea.codean))
                    {
                        xmlLineas += $@"<codean xsi:type=""soapenc:string"">{iLinea.codean}</codean>";
                    }
                    xmlLineas += $@"</LineaPedidoEntrada>";
                }
                xmlPedido = $@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent=""http://entrada.webservice.inlog.inlogconsulting.es"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <soapenv:Header />
                            <soapenv:Body>
                            <ent:callPedidoEntrada soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                        <dateTime xsi:type=""soapenc:string"">?</dateTime>
                                        <originatorName xsi:type=""soapenc:string"">{xId}</originatorName>
                                    </messageInfo>
                                    <trxId xsi:type=""soapenc:string"">{xId}</trxId>
                                </in0>
                                <in1 xsi:type=""bean:PedidoEntrada"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <accion xsi:type=""soapenc:string"">{xOC.accion}</accion>
                                    <almace xsi:type=""soapenc:string"">{xOC.almace}</almace>
                                    <fecasi xsi:type=""soapenc:string"">{xOC.fecasi}</fecasi>
                                    <fecha xsi:type=""soapenc:string"">{xOC.fecha}</fecha>
                                    <fecpre xsi:type=""soapenc:string"">{xOC.fecpre}</fecpre>
                                    <pedext xsi:type=""soapenc:string"">{xOC.pedext}</pedext>
                                    <proext xsi:type=""soapenc:string"">{xOC.proext}</proext>
                                    <propie xsi:type=""soapenc:string"">{xOC.propie}</propie>
                                    <sitped xsi:type=""soapenc:string"">{xOC.sitped}</sitped>
                                    <tipped xsi:type=""soapenc:string"">{xOC.tipped}</tipped>
                                    <totlin xsi:type=""soapenc:string"">{xOC.totlin}</totlin>
                                </in1>
                                <in2 xsi:type=""bean:ArrayOfLineaPedidoEntrada"" soapenc:arrayType=""bean:LineaPedidoEntrada[1]"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                {xmlLineas}    
                                </in2>
                            </ent:callPedidoEntrada>
                            </soapenv:Body>
                            </soapenv:Envelope>";

                soapEnvelopeXml.LoadXml(xmlPedido);

                f.Guardar_xml(xDatos, "OC", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos, "PedidoEntradaWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        public async Task<string> PS(mDatos xDatos, mPSCabecera xPS, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                string xmlLineas = "";
                string xmlPedido = "";
                XmlDocument soapEnvelopeXml = new XmlDocument();


                for (int i=0; i<xPS.Lineas.Count; i++)
                {
                    mPSLineas iLinea = xPS.Lineas[i];

                    xmlLineas += $@"<LineaPedidoSalida>
                                    <accion xsi:type=""soapenc:string"">{iLinea.accion}</accion>
                                    <artpro xsi:type=""soapenc:string"">{iLinea.artpro}</artpro>
                                    <artpv1 xsi:type=""soapenc:string"">{iLinea.artpv1}</artpv1>
                                    <artpv2 xsi:type=""soapenc:string"">{iLinea.artpv2}</artpv2>
                                    <artpvl xsi:type=""soapenc:string"">{iLinea.artpvl}</artpvl>  
                                    <canped xsi:type=""soapenc:string"">{iLinea.canped}</canped>
                                    <codlin xsi:type=""soapenc:string"">{iLinea.codlin}</codlin>
                                    <fecha xsi:type=""soapenc:string"">{iLinea.fecha}</fecha>
                                    <forfor xsi:type=""soapenc:string"">{iLinea.forfor}</forfor>
                                    <pedext xsi:type=""soapenc:string"">{iLinea.pedext}</pedext>
                                    <sitlin xsi:type=""soapenc:string"">{iLinea.sitlin}</sitlin>
                                    <tipfor xsi:type=""soapenc:string"">{iLinea.tipfor}</tipfor>
                                    </LineaPedidoSalida>";
                }
                xmlPedido = $@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent=""http://entrada.webservice.inlog.inlogconsulting.es"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <soapenv:Header/>
                            <soapenv:Body>
                            <ent:callPedidoSalida soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                        <dateTime xsi:type=""soapenc:string"">?</dateTime>
                                        <originatorName xsi:type=""soapenc:string"">?</originatorName>
                                    </messageInfo>
                                    <trxId xsi:type=""soapenc:string"">{xId}</trxId>
                                </in0>
                                <in1 xsi:type=""bean:PedidoSalida"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <accion xsi:type=""soapenc:string"">{xPS.accion}</accion>
                                    <almace xsi:type=""soapenc:string"">{xPS.almace}</almace>
                                    <copais xsi:type=""soapenc:string"">{xPS.copais}</copais>
                                    <codpos xsi:type=""soapenc:string"">{xPS.codpos}</codpos>
                                    <cliext xsi:type=""soapenc:string"">{xPS.cliext}</cliext>
                                    <cross xsi:type=""soapenc:string"">{xPS.cross}</cross>
                                    <dirent xsi:type=""soapenc:string"">{xPS.dirent}</dirent>
                                    <fecha xsi:type=""soapenc:string"">{xPS.fecha}</fecha>
                                    <guicar xsi:type=""soapenc:string"">{xPS.guicar}</guicar>
                                    <impadl xsi:type=""soapenc:string"">{xPS.impadl}</impadl>
                                    <indexp xsi:type=""soapenc:string"">{xPS.indexp}</indexp>
                                    <indnap xsi:type=""soapenc:string"">{xPS.indnap}</indnap>
                                    <indurg xsi:type=""soapenc:string"">{xPS.indurg}</indurg>
                                    <locali xsi:type=""soapenc:string"">{xPS.locali}</locali>
                                    <lotsec xsi:type=""soapenc:string"">{xPS.lotsec}</lotsec>
                                    <ordcar xsi:type=""soapenc:string"">{xPS.ordcar}</ordcar>
                                    <pedext xsi:type=""soapenc:string"">{xPS.pedext}</pedext>
                                    <propie xsi:type=""soapenc:string"">{xPS.propie}</propie>
                                    <provin xsi:type=""soapenc:string"">{xPS.provin}</provin>
                                    <sitped xsi:type=""soapenc:string"">{xPS.sitped}</sitped>
                                    <telefo xsi:type=""soapenc:string"">{xPS.telefo}</telefo>
                                    <tipped xsi:type=""soapenc:string"">{xPS.tipped}</tipped>
                                    <numexp xsi:type=""soapenc:string"">{xPS.numexp}</numexp>

                                </in1>
                                <in2 xsi:type=""bean:ArrayOfLineaPedidoSalida"" soapenc:arrayType=""bean:LineaPedidoSalida[1]"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                {xmlLineas}    
                                </in2>
			                    <in3 xsi:type=""bean:ArrayOfPedidoSalidaNS"" soapenc:arrayType=""bean:PedidoSalidaNS[]"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
         		                </in3>
                            </ent:callPedidoSalida>
                            </soapenv:Body>
                            </soapenv:Envelope>";

                soapEnvelopeXml.LoadXml(xmlPedido);

                f.Guardar_xml(xDatos, "PS", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos, "PedidoSalidaWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        public async Task<string> DevPro(mDatos xDatos, mDevProCabecera xDevol, Int32 xId, mAlmacenes Almacen)
        {
            Funciones f = new Funciones();

            string Respuesta = "";

            try
            {
                string xmlLineas = "";
                string xmlPedido = "";
                XmlDocument soapEnvelopeXml = new XmlDocument();


                for (int i = 0; i < xDevol.Lineas.Count; i++)
                {
                    mDevProLineas iLinea = xDevol.Lineas[i];

                    xmlLineas += $@"<LineaDevolucionProveedor>
                                    <accion xsi:type=""soapenc:string"">{iLinea.accion}</accion>
                                    <artpro xsi:type=""soapenc:string"">{iLinea.artpro}</artpro>
                                    <artpv1 xsi:type=""soapenc:string"">{iLinea.artpv1}</artpv1>
                                    <artpv2 xsi:type=""soapenc:string"">{iLinea.artpv2}</artpv2>
                                    <artpvl xsi:type=""soapenc:string"">{iLinea.artpvl}</artpvl>
                                    <cantap xsi:type=""soapenc:string"">{iLinea.cantap}</cantap>
                                    <cantna xsi:type=""soapenc:string"">{iLinea.cantna}</cantna>
                                    <codext xsi:type=""soapenc:string"">{iLinea.codext}</codext>
                                    <codlin xsi:type=""soapenc:string"">{iLinea.codlin}</codlin>
                                    <fecdev xsi:type=""soapenc:string"">{iLinea.fecdev}</fecdev>
                                    <fecha xsi:type=""soapenc:string"">{iLinea.fecha}</fecha>
                                    <sitlin xsi:type=""soapenc:string"">{iLinea.sitlin}</sitlin>
                                    </LineaDevolucionProveedor>";
                }

                xmlPedido = $@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ent=""http://entrada.webservice.inlog.inlogconsulting.es"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <soapenv:Header />
                            <soapenv:Body>
                            <ent:callDevolucionProveedor soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                <in0 xsi:type=""bean:MessageHeaderRequest"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <messageInfo xsi:type=""bean:MessageInfo"">
                                        <dateTime xsi:type=""soapenc:string"">?</dateTime>
                                        <originatorName xsi:type=""soapenc:string"">?</originatorName>
                                    </messageInfo>
                                    <trxId xsi:type=""soapenc:string"">{xId}</trxId>
                                </in0>
                                <in1 xsi:type=""bean:DevolucionProveedor"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                    <accion xsi:type=""soapenc:string"">{xDevol.accion}</accion>
                                    <almace xsi:type=""soapenc:string"">{xDevol.almace}</almace>
                                    <codext xsi:type=""soapenc:string"">{xDevol.codext}</codext>
                                    <fecdev xsi:type=""soapenc:string"">{xDevol.fecdev}</fecdev>
                                    <fecha xsi:type=""soapenc:string"">{xDevol.fecha}</fecha>
                                    <numdoc xsi:type=""soapenc:string"">{xDevol.numdoc}</numdoc>
                                    <proext xsi:type=""soapenc:string"">{xDevol.proext}</proext>
                                    <propie xsi:type=""soapenc:string"">{xDevol.propie}</propie>
                                    <sitcab xsi:type=""soapenc:string"">{xDevol.sitcab}</sitcab>
                                </in1>
                                <in2 xsi:type=""bean:ArrayOfLineaDevolucionProveedor"" soapenc:arrayType=""bean:LineaDevolucionProveedor[1]"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
                                {xmlLineas}    
                                </in2>
                                <in3 xsi:type=""bean:ArrayOfDevolucionProveedorNS"" soapenc:arrayType=""bean:DevolucionProveedorNS[]"" xmlns:bean=""http://beans.webservice.inlogconsulting.com.es"">
         		                </in3>
                            </ent:callDevolucionProveedor>
                            </soapenv:Body>
                            </soapenv:Envelope>";

                soapEnvelopeXml.LoadXml(xmlPedido);

                f.Guardar_xml(xDatos, "DevPro", soapEnvelopeXml.InnerXml, Almacen.Codigo);
                Respuesta = await GetRequest(xDatos, "DevolucionProveedorWS", soapEnvelopeXml, Almacen.URL);
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message;
            }
            return Respuesta;
        }

        private static async Task<string> GetRequest(mDatos xDatos, string xFunction, XmlDocument xXmlDocument, string xURL)
        {
            try
            {
                string Respuesta = "";
                HttpWebRequest request = CreateWebRequest(xDatos, xFunction, xURL);
                request.Timeout = 200000;
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    xXmlDocument.Save(stream);
                }
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {

                        XmlDocument xmlDoc1 = new XmlDocument();
                        string soapResult = rd.ReadToEnd();

                        xmlDoc1.LoadXml(@soapResult);

                        XmlNodeList itemNodes = xmlDoc1.GetElementsByTagName("mensaje");

                        if (itemNodes.Count > 0)
                        {
                            for (int i = 0; i < itemNodes.Count; i++)
                            {
                                Respuesta = itemNodes[i].InnerText;
                            }
                        }
                        return Respuesta;
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();

                        XmlDocument mDocument = new XmlDocument();
                        XmlNode mCurrentNode;

                        mDocument.LoadXml(text);
                        mCurrentNode = mDocument.DocumentElement;

                        return mCurrentNode.InnerText;
                    }
                }
            }
        }


        private static HttpWebRequest CreateWebRequest(mDatos xDatos, String xFuncion, string xURL)
        {
            string CadenaConexion = $"{xURL}/{xFuncion}";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(CadenaConexion);
            webRequest.Headers.Add("SOAPAction", @"POST");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.KeepAlive = true;
            webRequest.Timeout = 10000;
            return webRequest;
        }

    }
}
