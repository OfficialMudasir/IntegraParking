<xsl:stylesheet version = '1.0' 
	xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
	xmlns:ebl='urn:ebay:apis:eBLBaseComponents'>

<!--
This simple stylesheet will protect sensitive data from being output to the log file.
-->

<xsl:output 
	method="xml" 
	indent="yes"
	version="string"
	/>

<xsl:template match="*|@*|comment()|processing-instruction()|text()">
	<xsl:copy>
		<xsl:apply-templates select="*|@*|comment()|processing-instruction()|text()"/>
	</xsl:copy>
</xsl:template>

<xsl:template match="text()[ancestor::ebl:Password]">
	<xsl:text>*******</xsl:text>
</xsl:template>

<xsl:template match="text()[ancestor::ebl:Signature]">
	<xsl:text>*******</xsl:text>
</xsl:template>

<xsl:template match="text()[ancestor::ebl:CreditCardNumber]">
	<xsl:text>XXXX</xsl:text>
</xsl:template>

<xsl:template match="text()[ancestor::ebl:CVV2]">
	<xsl:text>XXX</xsl:text>
</xsl:template>

</xsl:stylesheet>