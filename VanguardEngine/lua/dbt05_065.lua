-- 穏やかな街の中 ケイオス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dbt05_041"))
end

function Activation()
	obj.RevealFromDeck(2)
	obj.AddToSoul({q.Location, l.Revealed, q.UnitType, u.Normal})
	obj.RearrangeOnBottom({q.Location, l.Revealed})
end