-- デザイアデビル ケンエン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetActivation("OnPlace")
end

function OnPlaceTrigger()
	return obj.PlayerMainPhase() and obj.IsApplicable()
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPlace()
	obj.LookAtTopOfDeck(1)
	local called = obj.SuperiorCall({q.Location, l.Looking})
	obj.Store(called)
	if obj.Exists({q.Location, l.Stored}) then
		local target = obj.GetID({q.Location, l.Stored})
		local timedTrigger = GiveAbility(GetID(), target)
		timedTrigger.SetTiming(a.OnEndPhase)
		timedTrigger.SetActivation("OnEndPhase")
		timedTrigger.SetResetTiming(p.UntilEndOfTurn)
		timedTrigger.SetResetTarget(target)
	end
end

function OnEndPhase()
	obj.Retire({q.Other, o.ThisFieldID})
end