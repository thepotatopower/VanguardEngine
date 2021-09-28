-- Crawl, you "Insects"!

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
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
	local p = 0
	if n == 1 then
		obj.SoulCharge(1)
		if obj.SoulCount() >= 5 then
			for i = obj.SoulCount(), 5, -5
			do
				p = p + 1
			end
			obj.ChooseAddTempPower(1, p * 10000)
		end
		if obj.SoulCount() >= 10 then
			obj.Draw(1)
		end
	end
	return 0
end