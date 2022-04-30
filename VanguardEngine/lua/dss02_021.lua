-- 極光戦姫 バラージ・ルトラ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetMovedFrom(l.RC, l.GC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.Applicable})
end

function Condition()
	return obj.HasPrison() and obj.Exists({q.Location, l.Applicable, q.Other, o.SameZone})
end

function Activation()
	obj.Imprison({q.Location, l.Applicable, q.Other, o.SameZone})
end