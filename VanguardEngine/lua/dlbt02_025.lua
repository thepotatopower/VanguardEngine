-- 羽ばたく音色 シグネ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Powerful)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnAttackTrigger")
	ability1.SetCondition("OnAttackCondition")
	ability1.SetCost("OnAttackCost")
	ability1.SetActivation("OnAttack")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetActivation("OnBattleEnds")
end

function OnAttackTrigger()
	return obj.IsBooster()
end

function OnAttackCondition()
	return obj.SoulCount() == 0
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttack()
	obj.Select({q.Location, l.PlayerUnits, q.Other, o.NotThisFieldID, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function OnBattleEndsTrigger()
	return obj.IsBooster()
end

function OnBattleEndsCondition()
	return obj.GetNumberOf("PowerfulExists", {l.Units}) > 0
end

function PowerfulExists(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.HasProperty(id, p.Powerful)
end

function OnBattleEnds()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.ChooseAddToDrop({q.Location, l.Soul, q.Count, 1})
end