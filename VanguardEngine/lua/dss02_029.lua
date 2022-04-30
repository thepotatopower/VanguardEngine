-- 奏天のの楽士 ルトティ

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
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.Attacking})
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bastion")})
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end