<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output indent="yes" method="html"/>
	<xsl:template match="ArchivosExcel">
		<html>	
			<body>
        <br></br>
        <h3>
          Log de Descarga Archivos Información de personal Protocolo Dipres <xsl:value-of select="Test/mes"/> <xsl:value-of select="Test/periodo"/>
        </h3>
      
           
        <table border="1" bordercolor="#000">
          <tr>
            <td>
              <b>Nombre Archivo</b>
            </td>
            <td>
              <b>Fecha de Descarga</b>
            </td>
            <td>
              <b>Analista</b>
            </td>
            <td>
              <b>Fecha de Corte</b>
            </td>
            <td>
              <b>Año</b>
            </td>
            <td>
              <b>Estado</b>
            </td>
            <td>
              <b>Vigencia</b>
            </td>
          </tr>
					<xsl:for-each select="EncabezadoReporte">
            <tr>
              <td><xsl:value-of select="NombreArchivo"/></td>
              <td width="200" style="mso-number-format:'@'">
                <xsl:value-of select="FechaDescarga"/>
              </td>
              <td >
                <xsl:value-of select="NombreUsuario"/>
              </td>
              <td >
                <xsl:choose>
                  <xsl:when test="mes = '1'">31 de marzo</xsl:when>
                  <xsl:when test="mes = '2'">30 de junio</xsl:when>
                  <xsl:when test="mes = '3'">30 de septiembre</xsl:when>
                  <xsl:when test="mes = '4'">31 de diciembre</xsl:when>
                  <xsl:when test="mes = '5'">Otra</xsl:when>
                  <xsl:otherwise>
                    <center>-</center>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td>
                <xsl:value-of select="periodo"/>
              </td>
              <td>
                <xsl:value-of select="estado"/>
              </td>
              <td>
                <xsl:value-of select="vigencia"/> 
              </td>
            </tr>	
					</xsl:for-each>
        </table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>  
