-- 旗艦竜 フラッグバーグ・ドラゴン

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.PlayerVC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	if obj.NumOfAttacks() >= 3 then
		obj.Draw(1)
	end
	if obj.NumOfAttacks() >= 4 then
		obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 2})
	end
	if obj.NumOfAttacks() >= 5 and obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerVC, q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfBattle)
		obj.AddPlayerValue(ps.GuardRestrict, 3, p.UntilEndOfBattle, false)
	end
end