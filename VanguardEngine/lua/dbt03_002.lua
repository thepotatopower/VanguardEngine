-- 砂塵の榴砲 ダスティン

function RegisterAbilities()
	-- placed on RC
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTriggerCondition("PlacedOnRCTrigger")
	ability1.SetActivationCondition("PlacedOnRCCondition")
	ability1.SetCost("PlacedOnRCCost")
	ability1.SetActivation("PlacedOnRC")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetTiming(a.OnAttack)
	ability2.SetTriggerCondition("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function PlacedOnRCTrigger()
	return obj.IsApplicable()
end

function PlacedOnRCCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_002")})
end

function PlacedOnRCCost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerHand, q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.PlayerHand, q.Count, 1})
end

function PlacedOnRC()
	obj.Draw(1)
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Grade, 2, q.Count, 1, q.Other, o.CanChoose, q.Other, o.GradeOrHigher})
end

function OnAttackTrigger()
	if obj.IsAttackingUnit() then
		obj.Track(GetID())
		return true
	end
	return false
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnAttack()
	if obj.IsSameZone(GetID()) then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, obj.NumEnemyOpenCircles() * 2000, p.UntilEndOfBattle)
	end
end