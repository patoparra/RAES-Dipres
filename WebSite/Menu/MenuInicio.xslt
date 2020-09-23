<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
    <xsl:param name="instrumento" select="1"/>
    <xsl:param name="perfil" select="'04'"/>
    <xsl:param name="usuario" select="'3281'"/>
    <xsl:param name="estado" select="'01'"/>
    <xsl:template match="/">
        <xsl:variable name="q">"</xsl:variable>
        <div class="seccion">
            <xsl:for-each select="//MenuTree[@Instrumento = $instrumento]/Menu[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
                <div class="subSeccion">
                    <fieldset>
                        <legend>
                            <xsl:value-of select="@Titulo"/>
                        </legend>
                        <xsl:if test="count(Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]) &gt; 0">
                            <ul>
                                <xsl:for-each select="Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
                                    <li>
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
                                        <xsl:choose>
                                            <xsl:when test="@ToolTip != ''">
                                                <div class="ui-silk-menu ui-silk-information someClass" style="cursor:pointer;display:inline;" >
                                                    <xsl:attribute name="tooltip">
                                                        <xsl:value-of select="@ToolTip"/>
                                                    </xsl:attribute>
                                                </div>
                                            </xsl:when>
                                            <xsl:otherwise>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                        <xsl:if test="count(Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]) &gt; 0">
                                            <ul>
                                                <xsl:for-each select="Item[(contains(@Perfiles, $perfil) or @Perfiles = '*') and (not(@Usuarios) or contains(@Usuarios, $usuario)) and (contains(@Estados, $estado) or @Estados = '*')]">
                                                    <li>
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
                                                        <xsl:choose>
                                                            <xsl:when test="@ToolTip != ''">
                                                                <div class="ui-silk-menu ui-silk-information someClass" style="cursor:pointer;display:inline;" >
                                                                    <xsl:attribute name="tooltip">
                                                                        <xsl:value-of select="@ToolTip"/>
                                                                    </xsl:attribute>
                                                                </div>
                                                            </xsl:when>
                                                            <xsl:otherwise>
                                                            </xsl:otherwise>
                                                        </xsl:choose>
                                                    </li>
                                                </xsl:for-each>
                                            </ul>
                                        </xsl:if>
                                    </li>
                                </xsl:for-each>
                            </ul>
                        </xsl:if>
                    </fieldset>
                </div>
            </xsl:for-each>
        </div>
    </xsl:template>
</xsl:stylesheet>
