-- 封焔竜 イダム

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.VCArms})
end

function Activation()
	local power = obj.GetNumberOf({q.Location, l.VCArms}) * 5000
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end