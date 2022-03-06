-- 麗し王子 メレディス

function RegisterAbilities()
	-- cont1
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Activation()
	obj.LookAtTopOfDeck(7)
	obj.Search({q.Location, l.Looking, q.Other, o.Order, q.Count, 1, q.Min, 0})
end