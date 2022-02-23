-- 忍竜 シャクガン

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyUnits, q.Grade, 3, q.Other, o.Attacked, q.Other, o.GradeOrHigher}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end