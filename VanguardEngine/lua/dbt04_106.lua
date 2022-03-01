-- 天瞳の斥候 トーヴェ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDriveCheck)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.RevealedTrigger, q.Other, o.This})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bastion")})
end

function Cost(check)
	if check then return obj.CanAddToDrop({q.Other, o.ThisFieldID}) end
	obj.AddToDrop({q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.FrontRowEnemyRC})
end

function Activation()
	obj.SendToBottom({q.Location, l.FrontRowEnemyRC})
end