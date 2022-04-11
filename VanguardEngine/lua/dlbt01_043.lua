--冴え渡るワードセンス フロール

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.RevealFromDeck(1)
	if not obj.Exists({q.Location, l.Revealed, q.Grade, 2, q.Other, o.Unit}) then
		obj.SuperiorCallToSpecificCircle({q.Location, l.Revealed}, FL.OpenCircle)
	end
	obj.RearrangeOnBottom({q.Location, l.Revealed})
end