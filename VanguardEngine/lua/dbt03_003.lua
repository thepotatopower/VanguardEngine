-- 強欲魔竜 グリードン

function RegisterAbilities()
	-- continuous
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.PlayerVC)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Continuous")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetLocation(l.PlayerVC)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetTriggerCondition("OnBattleEndsTrigger")
	ability2.SetCost("Cost")
	ability2.SetActivation("OnBattleEnds")
end

function Continuous()
	if obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt03_003")}) then
		obj.AddPlayerValue(ps.DamageNeededToLose, 7, p.Continuous)
	end
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanSB(2) and obj.CanAddToSoul({q.Location, l.PlayerRC, q.Other, o.Standing, q.Count, 4}) end
	obj.SoulBlast(2)
	obj.ChooseAddToSoul({q.Location, l.PlayerRC, q.Other, o.Standing, q.Count, 4})
end

function OnBattleEnds()
	if obj.IsSameZone() then
		obj.Stand({q.Location, l.PlayerVC, q.Other, o.This})
		if obj.SoulCount() >= 10 then
			obj.AddCardValue({q.Location, l.PlayerVC, q.Other, o.This}, cs.BonusPower, 15000, p.UntilEndOfTurn)
		end
	end
end