Dit is de practicum opgave van Siem van den Twele (659729) en Bart Hageman (6546374)

Open een nieuw scherm (File>Nieuw) of open (File>Openen) al bestaande .Schets bestanden om toegang te krijgen tot 
de paint interface.
 
Veranderingen t.o.v. de oorspronkelijke versie:
1. 	Interface overhaul. Scherm is nu een donkere versie van het origineel. 
	Icoontjes zijn aangepast naar icoontjes van https://materialdesignicons.com/ 
2.  Als een tool wordt gebruikt, bijvoorbeeld de rechthoek tool, wordt de outline van dat object weergegeven 
	met een gestippelde lijn.
3.  Cirkel tool - Rand/Gevuld toegevoegd.
4.  Tekenen maakt nu gebruik van een lijst. Elementen die worden toegevoegd door de gebruiker worden aan deze
	lijst toegevoegd, en telkens opnieuw getekend.
5. 	Gummen heeft nu twee functies:
	- Als er enkel wordt geklikt, d.w.z. de muis wordt niet bewogen, tussen het indrukken en het lostlaten 
	  van de muis, dan wordt het element waar op werd geklikt verwijderd uit de lijst en het plaatje opnieuw getekend.
	- Als er wel wordt gesleept, functioneert de gum net zoals de oude gum: Er wordt met een witte pen getekend.
6.  De lagen tool zorgt ervoor dat, op het moment dat er wordt geklikt op een object, het object vooraan komt te staan,
	en voor andere objecten wordt getekend. Als er nog een keer op geklikt wordt dan wordt het object helemaal achteraan
	neergezet.
7.  Er is nu een undo knop. De knop verwijdert het laatste object uit de lijst; de laastste actie wordt dus ongedaan
	gemaakt. Deze undo knop maakt echter geen gum acties ongedaan.
8. 	De kleur en dikte van de pen en de dikte van de gum kunnen worden aangepast via de inputs aan de onderkant van de
	paint interface.
9.  De clear knop wist de inhoud van de lijst en tekent dan opnieuw (leeg plaatje).
10. Er is nu een optie om het plaatje op te slaan als een .Schets bestand. In dit bestand staan de veschillende elementen
	die zijn getekend, met de benodigde parameters. Deze kan vervolgens weer worden uitgelezen met file>openen.
	