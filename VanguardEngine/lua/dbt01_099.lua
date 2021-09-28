-- Tier Square Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.FrontRowEnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.RevealedTrigger, q.UnitType, u.Trigger, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.CB, 1
	elseif n == 2 then
		return a.OnDriveCheck, p.HasPrompt, p.OncePerTurn, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Pentagleam Sorceress") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and obj.Exists(2) then
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

function Activate(n)
	if n == 1 then
		obj.Draw(1)
	elseif n == 2 then
		obj.ChooseSendToBottom(1)
	end
	return 0
end