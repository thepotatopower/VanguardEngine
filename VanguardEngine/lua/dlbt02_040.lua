-- 交わし合う視線 フェオドラ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Wilista")})
end

function Cost(check)
	if check then return obj.CanSendToBottom({q.Location, l.Drop, q.Other, o.Gem, q.Count, 1}) end
	obj.ChooseSendToBottom({q.Location, l.Drop, q.Other, o.Gem, q.Count, 1})
end

function Activation()
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end