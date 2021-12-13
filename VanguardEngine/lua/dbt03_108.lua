-- 廃滅の虚竜

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.Drop, q.Other, o.NormalOrder})
end

function Activation()
	obj.Select({q.Location, l.Drop, q.Other, o.NormalOrder, q.Count, 1, q.Min, 0})
	obj.PlayOrderWithoutCost({q.Location, l.Selected})
end