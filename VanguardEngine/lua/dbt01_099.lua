-- Tier Square Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.FrontRowEnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.RevealedTriggers, q.UnitType, u.Trigger, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnDriveCheck, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Pentagleam Sorceress") and obj.CanCB(2) then
			return true
		end
	elseif n == 2 then
		if not obj.Activated() and obj.IsRearguard() and obj.Exists(3) and obj.CanCB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(2)
	elseif n == 2 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.ChooseSendToBottom(1)
	end
	return 0
end