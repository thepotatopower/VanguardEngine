-- Aegismare Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPutOnGC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.PerfectGuard()
		if obj.Exists(2) then
			obj.Discard(1)
		end
	end
	return 0
end