function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.Deck, q.Other, o.World, q.Count, 1})
end

function Activation()
	obj.Search({q.Location, l.Deck, q.Other, o.World, q.Count, 1, q.Min, 0})
end

function Cont()
	if obj.IsAttackingUnit() or obj.IsBooster() and (obj.IsDarkNight() or obj.IsAbyssalDarkNight()) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end