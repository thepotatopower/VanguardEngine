-- 救命天使 ディグリエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnStand)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player, q.Other, o.SameColumn})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddToHand({q.Other, o.ThisFieldID})
end