-- パーフォレイトバーナー・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDiscard)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerTurn() and obj.IsBattlePhase() and obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddToSoul({q.Other, o.ThisFieldID})
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end