-- Earnest Wish, Hanael

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 0
end

function GetParam(n)
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.HasPrompt, p.Discard, 1
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
	end
	return 0
end