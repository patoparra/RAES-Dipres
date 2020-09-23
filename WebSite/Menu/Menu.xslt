<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  <xsl:param name="instrumento"/>
  <xsl:param name="perfil"/>
  <xsl:param name="usuario"/>
  <xsl:param name="estado"/>
  <xsl:template match="/">
    <xsl:variable name="q">"</xsl:variable>
    <div class="menuBar">
      <div class="homeItemBar" title="Página Inicio">
      </div>
      <xsl:for-each select="//MenuTree[@Instrumento = $instrumento]/Menu[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
        <div class="itemBar">
          <div class="titulo">
            <xsl:value-of select="@Titulo"/>
          </div>
          <xsl:if test="count(Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]) &gt; 0">
            <div class="items" style="display:none;">
              <ul class="itemList">
                <xsl:for-each select="Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
                  <li class="item">
                    <xsl:choose>
                      <xsl:when test="@Pagina != ''">
                        <a>
                          <xsl:attribute name="href">
                            <xsl:value-of select="@Pagina"/>
                          </xsl:attribute>
                        <xsl:value-of select="@Titulo"/>
                        </a>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="@Titulo"/>
                      </xsl:otherwise>
                    </xsl:choose>
                    <xsl:if test="count(Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]) &gt; 0">
                      <div class="subItems" style="display:none;">
                        <ul class="subItemsList">
                          <xsl:for-each select="Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
                            <li class="subItem">
                              <xsl:choose>
                                <xsl:when test="@Pagina != ''">
                                  <a>
                                    <xsl:attribute name="href">
                                      <xsl:value-of select="@Pagina"/>
                                    </xsl:attribute>
                                    <xsl:value-of select="@Titulo"/>
                                  </a>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="@Titulo"/>
                                </xsl:otherwise>
                              </xsl:choose>
                            </li>
                          </xsl:for-each>
                        </ul>
                      </div>
                    </xsl:if>
                  </li>
                </xsl:for-each>
              </ul>
            </div>
          </xsl:if>
        </div>
      </xsl:for-each>
    </div>
  </xsl:template>
</xsl:stylesheet>
