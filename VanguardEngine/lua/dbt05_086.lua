-- ティアーナイト フレーチェ

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
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.Attacked})
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.UnitType, u.Token})
end

function Activation()
	local power = obj.GetNumberOf({q.Location, l.PlayerUnits, q.UnitType, u.Token}) * 5000
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end