-- Crawl, you "Insects"!

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(1) then
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

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	end
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
			obj.ChooseAddTempPower(2, p * 10000)
		end
		if obj.SoulCount() >= 10 then
			obj.Draw(1)
		end
	end
	return 0
end